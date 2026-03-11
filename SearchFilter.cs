using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ZxInfoBot.publish.TgBot;

public static class SearchFilter
{
    
    static string Normalize(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        // перевод в нижний регистр
        input = input.ToLowerInvariant();

        // нормализация формы D — разделяем буквы и диакритику
        var normalizedString = input.Normalize(NormalizationForm.FormD);

        // оставляем только буквы и цифры, пробелы
        var chars = normalizedString
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            .Select(c => char.IsLetterOrDigit(c) ? c : ' ')
            .ToArray();

        // убираем лишние пробелы
        return string.Join(' ', new string(chars).Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }

    public static List<(string RawTitle, string Title, string SpectrumComputingUrl, string ZxInfoUrl, string Image)>
        FilterGames(List<(string RawTitle, string Title, string SpectrumComputingUrl, string ZxInfoUrl, string Image)> results, string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new();

        var normalizedQuery = Normalize(query);
        var queryWords = normalizedQuery.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (var r in results)
        {
            var titleWords = Normalize(r.RawTitle).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine($"TitleWords: {string.Join("|", titleWords)} | QueryWords: {string.Join("|", queryWords)}");
        }


        // одно слово — ищем полное совпадение
        if (queryWords.Length == 1)
        {
            var q = queryWords[0];

            return results
                .Where(g => !string.IsNullOrWhiteSpace(g.RawTitle) &&
                            Normalize(g.RawTitle) == q)
                .ToList();
        }

        // несколько слов — проверяем, что каждое слово встречается в названии
        return results.Where(g =>
        {
            if (string.IsNullOrWhiteSpace(g.RawTitle))
                return false;

            var titleWords = Normalize(g.RawTitle)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim()) // <-- обрезаем лишние пробелы
                .ToArray();

            return queryWords.All(qw => titleWords.Contains(qw.Trim()));
        }).ToList();
    }
}