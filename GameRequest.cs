using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ZxInfoBot
{
    public static class GameRequest
    {
        static async Task<List<GameRequestEntry>> Load(CancellationToken ct)
        {
            var result = new List<GameRequestEntry>();
            const int maxAttempts = 3;
             string url = @"https://docs.google.com/spreadsheets/d/12ZmERmZaVrJcek1fTU6Qqu9UWZZFnLL__6opEA7X9M0/export?format=tsv&gid=0";
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, timeoutCts.Token);

                Console.WriteLine($"Попытка {attempt}/{maxAttempts}, время: {DateTime.Now:HH:mm:ss}");
                Console.WriteLine($"Client.Timeout: {SharedHttpClient.Client.Timeout}");

                try
                {
                    using var response = await SharedHttpClient.Client.GetAsync(url, linkedCts.Token);
                    response.EnsureSuccessStatusCode();
                    string raw = await response.Content.ReadAsStringAsync(linkedCts.Token);

                    var lines = raw.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                    // Первая строка — заголовок, пропускаем
                    for (int i = 1; i < lines.Length; i++)
                    {
                        var columns = lines[i].Split('\t');
    
                        var entry = new GameRequestEntry();
                        entry.RowIndex          = i + 1; // +1 потому что строка 1 это заголовок
                        entry.Game              = GetColumn(columns, 0);
                        entry.Platform          = GetColumn(columns, 1);
                        entry.Rehoster          = GetColumn(columns, 2);
                        entry.RequestDate       = GetColumn(columns, 3);
                        entry.RehostStatus      = GetColumn(columns, 4);
                        entry.PlaythroughStatus = GetColumn(columns, 5);
                        entry.RehostStream      = GetColumn(columns, 6);
                        entry.PlaythroughStream = GetColumn(columns, 7);

                        if (!string.IsNullOrWhiteSpace(entry.Game)) result.Add(entry);
                    }
                    Console.WriteLine($"Загружено записей: {result.Count}");
                    return result;
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    Console.WriteLine("Загрузка отменена внешним токеном.");
                    return result;
                }
                catch (OperationCanceledException ex)
                {
                    Console.WriteLine($"Таймаут на попытке {attempt}/{maxAttempts}.");
                    Console.WriteLine($"  timeoutCts отменён: {timeoutCts.IsCancellationRequested}");
                    Console.WriteLine($"  внешний ct отменён: {ct.IsCancellationRequested}");
                    Console.WriteLine($"  Exception: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка на попытке {attempt}/{maxAttempts}: {ex.GetType().Name}: {ex.Message}");
                }

                if (attempt < maxAttempts)
                {
                    Console.WriteLine("Повтор через 1с...");
                    await Task.Delay(3000, ct);
                }
            }

            Console.WriteLine("Все попытки исчерпаны.");
            return result;
        }
        private static string GetColumn(string[] columns, int index)
        {
            if (index >= columns.Length) return "";
            return columns[index].Trim().Trim('"'); // убираем пробелы и кавычки если есть
        }
        public static async Task Show(ITelegramBotClient bot, long chatId, CancellationToken ct)
        {
            var allEntries = await Load(ct);
            var postponed = allEntries
                .FindAll(e => e.RehostStatus.Contains("Отложено", StringComparison.OrdinalIgnoreCase));
            foreach (var p in postponed)
            {
                var i = p.RehostStatus.Split(">");
                if (i.Length != 2) continue;
                if (int.TryParse(i[1].Trim(), out var rowIndex)) p.RowIndex = rowIndex;
            }

            var queueEntries = allEntries
                .FindAll(e => string.Equals(e.RehostStatus, "В очереди", StringComparison.OrdinalIgnoreCase));
            Console.WriteLine("------");
            foreach (var p in postponed)
            {
                Console.WriteLine($"{p.RowIndex}: {p.Game}, {p.Platform}, {p.RehostStatus}");
            }
            Console.WriteLine("------");
            foreach (var queueEntry in queueEntries)
            {
                Console.WriteLine($"{queueEntry.RowIndex}: {queueEntry.Game}, {queueEntry.Platform}, {queueEntry.RehostStatus}");
            }
            
            foreach (var p in postponed.Where(entry => entry.RowIndex > 0))
            {
                var insertIndex = queueEntries.FindIndex(entry => entry.RowIndex == p.RowIndex);
                if (insertIndex >= 0)
                {
                    queueEntries.Insert(insertIndex, p);
                }
            }

            // queueEntries.Sort((a, b) =>
            // {
            //     if (DateTime.TryParse(a.RequestDate, out var dateA) &&
            //         DateTime.TryParse(b.RequestDate, out var dateB))
            //         return dateA.CompareTo(dateB);
            //     return 0;
            // });

            var lines = new List<string>();
            foreach (var e in queueEntries)
                lines.Add($"{e.Game} [<i>{e.Platform}</i>] <b>{e.Rehoster}</b>");

            if (lines.Count == 0)
            {
                Console.WriteLine("Нет записей для отправки, сообщение не отправлено.");
                return;
            }

            await bot.SendMessage(
                chatId: chatId,
                text: string.Join('\n', lines),
                cancellationToken: ct,
                linkPreviewOptions: new LinkPreviewOptions { IsDisabled = true },
                parseMode: ParseMode.Html
            );
        }
    }

    public class GameRequestEntry
    {
        public int RowIndex = -1;
        [JsonProperty("Игра")] public string Game { get; set; }

        [JsonProperty("Платформа")] public string Platform { get; set; }

        [JsonProperty("РеХвостер")] public string Rehoster { get; set; }

        [JsonProperty("Дата реквеста")] public string RequestDate { get; set; }

        [JsonProperty("Статус реХвоста")] public string RehostStatus { get; set; }

        [JsonProperty("Статус прохождения")] public string PlaythroughStatus { get; set; }

        [JsonProperty("Ссылка на стрим-РеХвост")]
        public string RehostStream { get; set; }

        [JsonProperty("Ссылка на стрим с прохождением (если есть)")]
        public string PlaythroughStream { get; set; }
    }
}