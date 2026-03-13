using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ZxInfoBot.Models;

namespace ZxInfoBot;

public static class Api
{
    //  https://api.zxinfo.dk/v3/#/

    private static readonly HttpClient Client;

    static Api()
    {
        var handler = new HttpClientHandler()
        {
            SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13,
            AutomaticDecompression =
                System.Net.DecompressionMethods.GZip |
                System.Net.DecompressionMethods.Deflate,
            MaxConnectionsPerServer = 10
        };

        Client = new HttpClient(handler);

        Client.Timeout = TimeSpan.FromSeconds(15);

        Client.DefaultRequestHeaders.UserAgent.ParseAdd("ZxInfoBot/1.0");
        Client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        Client.DefaultRequestHeaders.ConnectionClose = true;
    }

    
    public static async Task<GameModel?> LoadGameData(string gameId)
    {
        var url = $"https://api.zxinfo.dk/v3/games/{gameId}?mode=tiny";

        try
        {
            Console.WriteLine($"Запрос: {url}");

            using var response = await Client.GetAsync(
                url,
                HttpCompletionOption.ResponseHeadersRead
            );

            Console.WriteLine($"Status: {response.StatusCode}");

            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync();

            using var reader = new StreamReader(stream);
            var content = await reader.ReadToEndAsync();

            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => args.ErrorContext.Handled = true,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var result = JsonConvert.DeserializeObject<GameModel>(content, settings);

            Console.WriteLine(result?.Source?.Title);

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка API: {ex}");
            return null;
        }
    }


    public static async Task<List<SimpleGameData>?> GetRandomGames(int count = 1)
    {
        var url = $@"https://api.zxinfo.dk/v3/games/random/{count}?mode=tiny&output=simple";
        try
        {
            Console.WriteLine($"Запрос: {url}");
            using var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var result = JsonConvert.DeserializeObject<List<SimpleGameData>>(content, settings);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка API: {ex.Message}");
            return null;
        }
    }

    public static async Task<InfoModel?> GetHitByName(string name)
    {
        var fullUrl = $"https://api.zxinfo.dk/v3/search?query={Uri.EscapeDataString(name)}&titlesonly=true&mode=tiny&size=100&offset=0&sort=rel_desc";
        try
        {
            Console.WriteLine($"Запрос: {fullUrl}");
            using var response = await Client.GetAsync(fullUrl);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var result = JsonConvert.DeserializeObject<InfoModel>(content, settings);
            Console.WriteLine($"Найдено: {result?.Hits?.HitsList?.Count ?? 0}");
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка API: {ex.Message}");
            return null;
        }
    }

    public static string GetExtendedName(Source source)
    {
        var year = source.OriginalYearOfRelease?.ToString() ?? "";
        year = year == string.Empty ? string.Empty : $"{year}";
        var model = $"\n{source.MachineType}" ?? "";
        var publisher = "";
        if (source.Publishers != null)
        {
            publisher = string.Join(", ", source.Publishers.Select(p => p != null ? p.Name : ""));
        }

        var contentType = source.ContentType.ToLower();
        var icon = contentType == "software" ? "🎮" : contentType == "book" ? "📖" : "❓";
        return $"{icon} <b>{source.Title}</b>\n<i>{year} - {publisher}</i>{model}";
    }

    public static string GetImagePath(Source source)
    {
        var path = source.Screens.Count > 0 ? source.Screens[0].Url : "";
        if (path == "") return EmptyImagePath;
        return $@"{MediaPath}{path}";
    }

    public static string GetZxInfoUrl(string id)
    {
        return $@"{ZxInfoPath}{id}";
    }

    public static string GetSpectrumComputingUrl(string id)
    {
        return $@"{SpectrumComputingPath}{id}";
    }

    public static async Task<SimpleGameData[]?> GetAuthorGames(string authorName)
    {
        var url = $@"https://api.zxinfo.dk/v3/authors/{authorName}/games?mode=tiny&size=50&offset=0&sort=rel_desc&output=simple";
        try
        {
            Console.WriteLine($"Запрос: {url}");
            using var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var result = JsonConvert.DeserializeObject<SimpleGameData[]>(content, settings);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка API: {ex.Message}");
            return null;
        }
    }
}