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

    public static readonly HttpClient Client;

    static Api()
    {

        Client = SharedHttpClient.Client;
    }


    public static async Task<GameModel?> LoadGameData(string gameId)
    {
        var url = $"https://api.zxinfo.dk/v3/games/{gameId}?mode=tiny";
    
        try
        {
            Console.WriteLine($"Запрос: {url}");

            using var request = new HttpRequestMessage(HttpMethod.Get, url); // внутри try
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
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
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Console.WriteLine($"Запрос: {url}");
            using var response = await Client.SendAsync(request);
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
        
            using var request = new HttpRequestMessage(HttpMethod.Get, fullUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
            using var response = await Client.SendAsync(request); // <-- было GetAsync
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
        var pubUrl = $"https://api.zxinfo.dk/v3/publishers/{authorName}/games?mode=tiny&size=50&offset=0&sort=rel_desc&output=simple";
        var authorUrl = $"https://api.zxinfo.dk/v3/authors/{authorName}/games?mode=tiny&size=50&offset=0&sort=rel_asc&output=simple";

        try
        {
            Console.WriteLine($"Запрос: {authorUrl}");
            Console.WriteLine($"Запрос: {pubUrl}");

            // Каждый запрос — свой HttpRequestMessage
            using var authorRequest = new HttpRequestMessage(HttpMethod.Get, authorUrl);
            authorRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var pubRequest = new HttpRequestMessage(HttpMethod.Get, pubUrl);
            pubRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var authors = Client.SendAsync(authorRequest);
            var publishers = Client.SendAsync(pubRequest);
            var responses = await Task.WhenAll(authors, publishers);

            var result = new List<SimpleGameData>();
            var serializer = new JsonSerializer();

            foreach (var message in responses)
            {
                message.EnsureSuccessStatusCode();
                await using var stream = await message.Content.ReadAsStreamAsync();
                using var sr = new StreamReader(stream);
                using var reader = new JsonTextReader(sr);
                var data = serializer.Deserialize<SimpleGameData[]>(reader);
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        if (result.All(x => x.Id != item.Id))
                            result.Add(item);
                    }
                }
            }

            result.Sort((x, y) => string.Compare(x.Title, y.Title, StringComparison.Ordinal));
            return result.ToArray();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка API: {ex.Message}");
            return null;
        }
    }
}