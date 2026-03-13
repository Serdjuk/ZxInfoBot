using System;
using System.Collections.Generic;
using System.Net.Http;
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

            try
            {
                string url = "https://docs.google.com/spreadsheets/d/12ZmERmZaVrJcek1fTU6Qqu9UWZZFnLL__6opEA7X9M0/gviz/tq?tqx=out:json&gid=0";

                using var client = new HttpClient
                {
                    Timeout = TimeSpan.FromSeconds(100) // фиксируем 100 секунд
                };

                string raw = await client.GetStringAsync(url, ct); // передаём токен сюда

                int start = raw.IndexOf('{');
                int end = raw.LastIndexOf('}');
                if (start < 0 || end <= start) return result;

                string json = raw.Substring(start, end - start + 1);
                JObject obj = JObject.Parse(json);

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

                    if (hasValue)
                        result.Add(entry);
                }

                Console.WriteLine($"Загружено записей: {result.Count}");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Загрузка таблицы была прервана (таймаут или отмена).");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при загрузке таблицы:");
                Console.WriteLine(ex.Message); // выводим только короткое сообщение
            }

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
                {
                    return dateA.CompareTo(dateB);
                }

                return 0;
            });

            var lines = new List<string>();
            foreach (var e in queueEntries)
            {
                lines.Add($"{e.Game} | <i>{e.Platform}</i> | <b>{e.Rehoster}</b>");
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