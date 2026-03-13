using System;
using System.Collections.Generic;
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
            string url = "https://docs.google.com/spreadsheets/d/12ZmERmZaVrJcek1fTU6Qqu9UWZZFnLL__6opEA7X9M0/gviz/tq?tqx=out:json&gid=0";

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                // Объявляем ДО try — чтобы были видны в catch
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, timeoutCts.Token);

                Console.WriteLine($"Попытка {attempt}/{maxAttempts}, время: {DateTime.Now:HH:mm:ss}");
                Console.WriteLine($"Client.Timeout: {SharedHttpClient.Client.Timeout}");

                try
                {
                    using var response = await SharedHttpClient.Client.GetAsync(url, linkedCts.Token);
                    response.EnsureSuccessStatusCode();
                    string raw = await response.Content.ReadAsStringAsync(linkedCts.Token);

                    int start = raw.IndexOf('{');
                    int end = raw.LastIndexOf('}');
                    if (start < 0 || end <= start)
                    {
                        Console.WriteLine("Не удалось найти JSON в ответе Google Sheets.");
                        return result;
                    }

                    string json = raw.Substring(start, end - start + 1);
                    var obj = JObject.Parse(json);

                    var rows = obj["table"]?["rows"] as JArray;
                    if (rows == null) return result;

                    foreach (var row in rows)
                    {
                        var cells = row["c"] as JArray;
                        if (cells == null) continue;

                        var entry = new GameRequestEntry();
                        bool hasValue = false;

                        entry.Game = GetCellValue(cells, 0, ref hasValue);
                        entry.Platform = GetCellValue(cells, 1, ref hasValue);
                        entry.Rehoster = GetCellValue(cells, 2, ref hasValue);
                        entry.RequestDate = GetCellValue(cells, 3, ref hasValue);
                        entry.RehostStatus = GetCellValue(cells, 4, ref hasValue);
                        entry.PlaythroughStatus = GetCellValue(cells, 5, ref hasValue);
                        entry.RehostStream = GetCellValue(cells, 6, ref hasValue);
                        entry.PlaythroughStream = GetCellValue(cells, 7, ref hasValue);

                        if (hasValue) result.Add(entry);
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
                    await Task.Delay(1000, ct);
                }
            }

            Console.WriteLine("Все попытки исчерпаны.");
            return result;
        }

        public static async Task Show(ITelegramBotClient bot, long chatId, CancellationToken ct)
        {
            var allEntries = await Load(ct);

            var queueEntries = allEntries
                .FindAll(e => string.Equals(e.RehostStatus, "В очереди", StringComparison.OrdinalIgnoreCase));

            queueEntries.Sort((a, b) =>
            {
                if (DateTime.TryParse(a.RequestDate, out var dateA) &&
                    DateTime.TryParse(b.RequestDate, out var dateB))
                    return dateA.CompareTo(dateB);
                return 0;
            });

            var lines = new List<string>();
            foreach (var e in queueEntries)
                lines.Add($"{e.Game} | <i>{e.Platform}</i> | <b>{e.Rehoster}</b>");

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

        private static string GetCellValue(JArray cells, int index, ref bool hasValue)
        {
            if (index >= cells.Count) return "";

            var cell = cells[index];
            if (cell == null || cell.Type != JTokenType.Object) return "";

            string value = cell["v"]?.ToString() ?? "";
            if (!string.IsNullOrEmpty(value)) hasValue = true;

            return value;
        }
    }

    public class GameRequestEntry
    {
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