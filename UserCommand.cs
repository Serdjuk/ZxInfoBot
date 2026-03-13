using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ZxInfoBot;

public static class UserCommand
{
    const string FileName = @"userCommands.txt";

    public static async Task Show(string chatMessage, ITelegramBotClient bot, long chatId, CancellationToken ct, int messageId)
    {
        var userCommandLines = await Program.Load(FileName);
        if (userCommandLines == null || userCommandLines.Length == 0) return;
        
        var commands = userCommandLines
            .Select(s => s.Trim().Split('\t'))
            .ToDictionary(k => k[0], v => v[1]);
        var values = commands
            .OrderByDescending(pair => pair.Key.Length)
            .FirstOrDefault(pair => chatMessage.StartsWith(pair.Key, StringComparison.OrdinalIgnoreCase))
            .Value;
        if  (string.IsNullOrEmpty(values)) return;
        string message = values.Replace("\\n", "\n");
        // await bot.DeleteMessage(chatId, messageId, ct);
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