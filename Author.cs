using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ZxInfoBot;

public static class Author
{
    public static async Task Show(string query, ITelegramBotClient bot, long chatId, CancellationToken ct)
    {
        try
        {
            query = query.Replace(RequestTokenAuthor, string.Empty).Trim();
            if (query.Length < 2) return;
            var model = await Api.GetAuthorGames(query);
            if (model == null) return;
            var lines = new List<string> { "Games by the author:" };

            foreach (var game in model)
            {
                var gameName = game.Title;
                var gameLink = Api.GetSpectrumComputingUrl(game.Id);
                var link = $@"<a href=""{gameLink}"">{gameName}</a>";
                Console.WriteLine($"link: {link}");
                lines.Add(link);
            }
            
            await bot.SendMessage(
                chatId: chatId,
                text:string.Join('\n', lines),  
                cancellationToken: ct,
                linkPreviewOptions: new LinkPreviewOptions { IsDisabled = true },
                parseMode: ParseMode.Html
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}