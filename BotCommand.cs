namespace ZxInfoBot;


public static class BotCommand
{
    public const string RequestTokenGame = "/game";
    public const string RequestTokenList = "/list";
    public const string RequestTokenHelp = "/help";
    public const string RequestTokenExit = "/exit";
    public const string RequestTokenRandom = "/rnd";
    public const string RequestTokenAuthor = "/author";
    public static readonly string[] RequestTokenRequest = ["/request","/очередь"];

    public const string EmptyImagePath = @"https://dummyimage.com/150x150/cccccc/000000.png&text=No+Image";
    
    public const string SpectrumComputingPath = @"https://spectrumcomputing.co.uk/entry/";
    public const string ZxInfoPath = @"https://zxinfo.dk/details/";
    public const string MediaPath = @"https://zxinfo.dk/media/";
    
    public const string HelpText = @"<code>
🤖 Bot Commands
/game [title] – Search for games (includes images).
/list [title] – Search for games (text list only).
/rnd – Show a random game with an image.
/help – Show this help message.
/exit – Disable the bot.

🔍 Search Logic

1️⃣ Single Word: The bot looks for an exact match of the game title.
Example: /game Saboteur - will find the game titled ""Saboteur"" or ""Saboteur!"".

2️⃣ Multiple Words: The bot finds games containing all provided words in any order.
Example: /game dizzy and - will find ""Dizzy 3 and a Half"".
Example: /list magazin x - will find ""X-Magazin 00"", ""X-Magazín 01"", etc.

⚙️ Good to Know
Flexible Formatting: The search ignores punctuation, hyphens, and extra spaces.
Smart Matching: Accents and diacritic marks (like í or ñ) are treated as standard letters.
Case Insensitive: No need to worry about CAPS or lowercase.
</code>
";
}
