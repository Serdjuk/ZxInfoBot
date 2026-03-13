using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ZxInfoBot;

public static class Help
{
    const string FileName = @"help.txt";

    public static async Task Show(ITelegramBotClient bot, long chatId, CancellationToken ct)
    {
        var lines = await Program.Load(FileName);
        if (lines == null || lines.Length == 0) return;
        var message = string.Join('\n',lines) ;
        try
        {
            await bot.SendMessage(
                chatId: chatId,
                text: message,
                parseMode: ParseMode.Html,
                cancellationToken: ct
            );
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
    }
}