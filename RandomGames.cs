using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ZxInfoBot.publish.TgBot;

public static class RandomGames
{
    public static async Task GetRandomGames(ITelegramBotClient bot, long chatId, CancellationToken ct)
    {
        try
        {
            var model = await Api.GetRandomGames();

            if (model == null) return;
            var results = new List<(string RawTitle, string Title, string SpectrumComputingUrl, string ZxInfoUrl, string Image)>();
            foreach (var data in model)
            {
                var game = await Api.LoadGameData(data.Id);
                Console.WriteLine($"{game.Id}, {game.Source.Title}");
                var source = game.Source;
                var rawTitle = source.Title;
                var title = Api.GetExtendedName(source);
                var scUrl = Api.GetSpectrumComputingUrl(game.Id);
                var ziUrl = Api.GetZxInfoUrl(game.Id);
                var imageUrl = Api.GetImagePath(source);
                results.Add((RawTitle: rawTitle, Title: title, SpectrumComputingUrl: scUrl, ZxInfoUrl: ziUrl, Image: imageUrl));
            }

            Show(results, bot, chatId, ct);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }


    static async void Show(List<(string RawTitle, string Title, string SpectrumComputingUrl, string ZxInfoUrl, string Image)> results, ITelegramBotClient bot, long chatId, CancellationToken ct)
    {
        try
        {
            foreach (var game in results)
            {
                var keyboard = new InlineKeyboardMarkup([
                    [
                        InlineKeyboardButton.WithUrl("SpectrumComputing", game.SpectrumComputingUrl),
                        InlineKeyboardButton.WithUrl("ZxInfo", game.ZxInfoUrl)
                    ]
                ]);


                await bot.SendPhoto(
                    chatId: chatId,
                    photo: game.Image,
                    caption: game.Title,
                    replyMarkup: keyboard,
                    cancellationToken: ct,
                    parseMode: ParseMode.Html
                );
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}