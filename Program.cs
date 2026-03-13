using System;
using System.IO;
using System.Threading.Tasks;
using ZxInfoBot.publish.TgBot;

namespace ZxInfoBot
{
    //   dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
    class Program
    {
        static async Task Main(string[] args)
        {
            await Bot.StartBot();
        }

        public static async Task<string?> Load(string fileName)
        {
            try
            {
                var exePath = AppContext.BaseDirectory; // папка, где лежит exe
                var tokenPath = Path.Combine(exePath, fileName);

                if (!File.Exists(tokenPath))
                {
                    Console.WriteLine($"Файл {tokenPath} не найден!");
                    Console.WriteLine($"Поместите '{fileName}'  в папку с exe и попробуйте снова.");
                    Console.ReadLine();
                    return null;
                }

                var message = (await File.ReadAllTextAsync(tokenPath)).Trim();

                if (string.IsNullOrWhiteSpace(message))
                {
                    Console.WriteLine($"Файл {fileName} пустой или содержит только пробелы!");
                    Console.ReadLine();
                    return null;
                }

                Console.WriteLine($"The file '{fileName}' has been downloaded successfully.");
                return message;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FATAL] {ex}");
                Console.ReadLine();
            }

            return null;
        }
    }
}