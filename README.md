
# ZX Spectrum Telegram Bot 🕹️

A console-based Telegram bot that allows users to search for and discover ZX Spectrum games and magazines.

## 🚀 Features
- **Search by Title:** Find specific games with preview images or as a text list.
- **Random Pick:** Discover classic titles using the random generator.
- **Rich Data:** Pulls information directly from the [ZXInfo API](https://api.zxinfo.dk/v3/).
- **External Links:** Automatically generates links to [SpectrumComputing.co.uk](https://spectrumcomputing.co.uk) and [ZXInfo.dk](https://zxinfo.dk/) for every game.

---

## 🛠️ Setup & Installation

1. **Create a Bot:** Message [@BotFather](https://t.me) on Telegram to create your bot and get an **API Token**.
2. **Configure Token:**
    - Create a file named `bot.token` in the same folder as the `.exe` file.
    - Paste your API Token into this file and save it.
    - *Tip: You can create a text file, rename it (including the extension), and edit it via Notepad or Far Manager (F4).*
3. **Run:** Launch the `.exe` file. If the token is valid, the bot will start working immediately.

---

## 🤖 Bot Commands


| Command                                   | Description                                  |
|:------------------------------------------|:---------------------------------------------|
| `/game [title]`                           | Search for games (shows results with images) |
| `/list [title]`                           | Search for games (shows results as a list)   |
| `/rnd`                                    | Display a random game with an image          |
| `/help`                                   | Show the help message                        |
| `/exit`                                   | Shut down the bot                            |
| `/author`                                 | Show games by this author (max 50)           |

## 📂 Custom User Commands (External File)

The bot can process custom responses from an external file `userCommands.txt` located in the application folder.

### Configuration:
- Create `userCommands.txt` in the same folder as the `.exe`.
- **Format:** `[command][TAB][user text]`
- Commands can start with `/` or `$`.
- The user text must be in **ONE LINE**.
- To add a line break in the response, use the `\n` sequence.
- Supports **ParseMode.Html** (tags like `<b>`, `<i>`, `<a>` are allowed).

### Example of `userCommands.txt`:
```text
/links	<b>Useful links:</b>\nhttps://rgg.land\nhttps://wiki.specnext.dev
$rules	1. No spam\n2. Be cool
```
---


## 🔍 Search Logic

The bot uses a smart search algorithm to make finding titles easier:

1. **Single Word Search:**
   If you enter one word, the bot searches for an **exact match**.
    *   *Example:* `/game Saboteur` will find the exact title "Saboteur" or "Saboteur!".

2. **Multi-Word Search:**
   If you enter multiple words, the bot finds games containing **all provided words** in any order.
    *   *Example:* `/game dizzy and` → finds "Dizzy 3 and a Half".
    *   *Example:* `/list magazin x` → finds "X-Magazin 00", "X-Magazín 01", etc.

### Key Features:
*   **Fuzzy Matching:** Ignores punctuation, hyphens, and extra spaces.
*   **Diacritic Insensitive:** Accents (like `í` or `ñ`) are treated as standard characters.
*   **Case Insensitive:** Search is not affected by capital or lowercase letters.

---

## 📚 Data Sources
- **Database:** [ZXInfo](https://zxinfo.dk)
- **API:** [ZXInfo API v3](https://api.zxinfo.dk/v3/)
- **Additional Info:** [Spectrum Computing](https://spectrumcomputing.co.uk)
