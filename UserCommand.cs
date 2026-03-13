using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ZxInfoBot;

public class UserCommand
{
    const string FileName = @"userCommand.txt";

    public static async Task Show(string chatMessage, ITelegramBotClient bot, long chatId, CancellationToken ct)
    {
        var userCommands = await Program.Load(FileName) ?? "";
        var commands = userCommands.Split('\t');



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