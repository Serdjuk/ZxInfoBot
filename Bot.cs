using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ZxInfoBot.publish.TgBot;

public static class Bot
{
    public static async Task StartBot()
    {
        try
        {
            var exePath = AppContext.BaseDirectory; // папка, где лежит exe
            var tokenPath = Path.Combine(exePath, "bot.token");

            if (!File.Exists(tokenPath))
            {
                Console.WriteLine($"Файл {tokenPath} не найден!");
                Console.WriteLine("Поместите bot.token в папку с exe и попробуйте снова.");
                Console.ReadLine();
                return;
            }

            var botToken = (await File.ReadAllTextAsync(tokenPath)).Trim();

            if (string.IsNullOrWhiteSpace(botToken))
            {
                Console.WriteLine("Файл bot.token пустой или содержит только пробелы!");
                Console.ReadLine();
                return;
            }

            var bot = new TelegramBotClient(botToken);

            try
            {
                var me = await bot.SendRequest(new GetMeRequest());
                Console.WriteLine($"Бот успешно запущен: @{me.Username}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при подключении к Telegram: {ex.Message}");
                Console.ReadLine();
                return;
            }

            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // получать все типы обновлений
            };

            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cts.Token
            );

            Console.WriteLine("Бот запущен. Нажмите Enter для выхода.");
            Console.ReadLine();

            await cts.CancelAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FATAL] {ex}");
            Console.ReadLine();
        }
    }


    static async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        if (update.Message?.Text == null)
            return;

        var chatId = update.Message.Chat.Id;
        var messageId = update.Message.MessageId;
        var query = update.Message.Text.Trim();

        var viewMode = ViewMode.None;

        if (query.StartsWith(RequestTokenExit, StringComparison.OrdinalIgnoreCase)) await Exit(bot, update.Message);

        if (query.StartsWith(RequestTokenHelp, StringComparison.OrdinalIgnoreCase))
        {
            await Help.Show(bot, chatId, ct);
        }
        
        await UserCommand.Show(query, bot,chatId, ct, messageId);

        if (query.StartsWith(RequestTokenRandom, StringComparison.OrdinalIgnoreCase))
        {
            await RandomGames.GetRandomGames(bot, chatId, ct);
            return;
        }

        if (query.StartsWith(RequestTokenGame, StringComparison.OrdinalIgnoreCase))
        {
            query = query.Replace(RequestTokenGame, string.Empty).Trim();
            viewMode = string.IsNullOrEmpty(query) ? ViewMode.None : ViewMode.WithImage;
        }

        if (query.StartsWith(RequestTokenList, StringComparison.OrdinalIgnoreCase))
        {
            query = query.Replace(RequestTokenList, string.Empty).Trim();
            viewMode = string.IsNullOrEmpty(query) ? ViewMode.None : ViewMode.TextOnly;
        }

        if (viewMode == ViewMode.None || query.Length < 2) return;
        // var q = query.Split(' ').Select(s => s.Trim().ToLower());
        // query = string.Join(' ', new HashSet<string>(q));
        // Console.WriteLine($"Query: {query}");
        var model = await Api.GetHitByName(query);
        var results = new List<(string RawTitle, string Title, string SpectrumComputingUrl, string ZxInfoUrl, string Image)>();
        if (model != null)
        {
            foreach (var hit in model.Hits.HitsList)
            {
                var rawTitle = hit.Source.Title;
                var title = Api.GetExtendedName(hit.Source);
                var scUrl = Api.GetSpectrumComputingUrl(hit.Id);
                var ziUrl = Api.GetZxInfoUrl(hit.Id);
                var imageUrl = Api.GetImagePath(hit.Source);
                results.Add((RawTitle: rawTitle, Title: title, SpectrumComputingUrl: scUrl, ZxInfoUrl: ziUrl, Image: imageUrl));
            }
        }

        results = SearchFilter.FilterGames(results, query);
        Console.WriteLine($"Показано: {results.Count}");

        foreach (var game in results)
        {
            var keyboard = new InlineKeyboardMarkup([
                [
                    InlineKeyboardButton.WithUrl("SpectrumComputing", game.SpectrumComputingUrl),
                    InlineKeyboardButton.WithUrl("ZxInfo", game.ZxInfoUrl)
                ]
            ]);

            try
            {
                switch (viewMode)
                {
                    case ViewMode.TextOnly:
                        await bot.SendMessage(
                            chatId: chatId,
                            text: game.Title,
                            parseMode: ParseMode.Html,
                            replyMarkup: keyboard,
                            cancellationToken: ct
                        );
                        break;
                    case ViewMode.WithImage:
                        await bot.SendPhoto(
                            chatId: chatId,
                            photo: game.Image,
                            caption: game.Title,
                            replyMarkup: keyboard,
                            cancellationToken: ct,
                            parseMode: ParseMode.Html
                        );
                        break;
                    case ViewMode.None:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (ApiRequestException ex)
            {
                Console.WriteLine($"'{game.Title}': {ex.Message}");
            }
        }
    }


    static Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken ct)
    {
        Console.WriteLine($"[ERROR] {exception}");
        return Task.CompletedTask;
    }


    static async Task Exit(ITelegramBotClient bot, Message message)
    {
        try
        {
            if (message.From != null)
            {
                var chatMember = await bot.GetChatMember(
                    chatId: message.Chat.Id,
                    userId: message.From.Id
                );

                var isPrivileged = chatMember.Status is ChatMemberStatus.Administrator
                    or ChatMemberStatus.Creator;
                if (isPrivileged)
                {
                    Environment.Exit(0);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
    }
}