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

    public static async Task Show(string chatMessage, ITelegramBotClient bot, long chatId, CancellationToken ct)
    {
        var userCommands = await Program.Load(FileName) ?? "";
        if (userCommands.Length == 0) return;
        var commands = userCommands
            .Split('\n')
            .Select(s => s.Trim().Split('\t'))
            .ToDictionary(k => k[0], v => v[1]);
        var message = commands
            .OrderByDescending(pair => pair.Key.Length)
            .FirstOrDefault(pair => chatMessage.StartsWith(pair.Key, StringComparison.OrdinalIgnoreCase))
            .Value;
        if  (string.IsNullOrEmpty(message)) return;
        
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