// YApi QuickType插件生成，具体参考文档:https://plugins.jetbrains.com/plugin/18847-yapi-quicktype/documentation

using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ZxInfoBot.Models
{
    public partial class InfoModel
    {
        [JsonProperty("_shards")]
        public Shards Shards { get; set; }

        [JsonProperty("hits")]
        public Hits Hits { get; set; }

        [JsonProperty("took")]
        public long Took { get; set; }

        [JsonProperty("timed_out")]
        public bool TimedOut { get; set; }
    }

    public partial class Hits
    {
        [JsonProperty("hits")]
        public List<Hit> HitsList { get; set; }

        [JsonProperty("total")]
        public Total Total { get; set; }

        [JsonProperty("max_score")]
        public double MaxScore { get; set; }
    }

    public partial class Hit
    {
        [JsonProperty("_index")]
        public string Index { get; set; }

        [JsonProperty("_source")]
        public Source Source { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("sort")]
        public List<Sort> Sort { get; set; }

        [JsonProperty("_score")]
        public double Score { get; set; }
    }

    public partial class Source
    {
        [JsonProperty("zxinfoVersion")]
        public string ZxinfoVersion { get; set; }

        [JsonProperty("availability")]
        public string Availability { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("releases")]
        public List<Release> Releases { get; set; }

        [JsonProperty("genreType")]
        public string GenreType { get; set; }

        [JsonProperty("score")]
        public Score Score { get; set; }

        [JsonProperty("additionalDownloads")]
        public List<AdditionalDownload> AdditionalDownloads { get; set; }

        [JsonProperty("xrated")]
        public long Xrated { get; set; }

        [JsonProperty("screens")]
        public List<Screen> Screens { get; set; }

        [JsonProperty("originalYearOfRelease", NullValueHandling = NullValueHandling.Ignore)]
        public long? OriginalYearOfRelease { get; set; }

        [JsonProperty("genre")]
        public string Genre { get; set; }

        [JsonProperty("publishers")]
        public List<Publisher?>? Publishers { get; set; }

        [JsonProperty("genreSubType", NullValueHandling = NullValueHandling.Ignore)]
        public string GenreSubType { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("machineType")]
        public string? MachineType { get; set; }

        [JsonProperty("authors")]
        public List<Author> Authors { get; set; }

        [JsonProperty("originalDayOfRelease", NullValueHandling = NullValueHandling.Ignore)]
        public long? OriginalDayOfRelease { get; set; }

        [JsonProperty("originalMonthOfRelease", NullValueHandling = NullValueHandling.Ignore)]
        public long? OriginalMonthOfRelease { get; set; }
    }

    public partial class AdditionalDownload
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
        public string Language { get; set; }

        [JsonProperty("encodingScheme", NullValueHandling = NullValueHandling.Ignore)]
        public string EncodingScheme { get; set; }
    }

    public partial class Author
    {
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public Country? Country { get; set; }

        [JsonProperty("groupName", NullValueHandling = NullValueHandling.Ignore)]
        public string GroupName { get; set; }

        [JsonProperty("groupType", NullValueHandling = NullValueHandling.Ignore)]
        public GroupTypeEnum? GroupType { get; set; }

        [JsonProperty("notes", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> Notes { get; set; }

        [JsonProperty("groupCountry", NullValueHandling = NullValueHandling.Ignore)]
        public Country? GroupCountry { get; set; }

        [JsonProperty("authorSeq", NullValueHandling = NullValueHandling.Ignore)]
        public long? AuthorSeq { get; set; }

        [JsonProperty("roles", NullValueHandling = NullValueHandling.Ignore)]
        public List<Role> Roles { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("labelType", NullValueHandling = NullValueHandling.Ignore)]
        public LabelType? LabelType { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public AuthorType? Type { get; set; }
    }

    public partial class Role
    {
        [JsonProperty("roleName")]
        public string RoleName { get; set; }

        [JsonProperty("roleType")]
        public string RoleType { get; set; }
    }

    public partial class Publisher
    {
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        [JsonProperty("notes", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> Notes { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("labelType", NullValueHandling = NullValueHandling.Ignore)]
        public GroupTypeEnum? LabelType { get; set; }

        [JsonProperty("publisherSeq", NullValueHandling = NullValueHandling.Ignore)]
        public long? PublisherSeq { get; set; }
    }

    public partial class Release
    {
        [JsonProperty("publishers")]
        public List<Publisher> Publishers { get; set; }

        [JsonProperty("files")]
        public List<AdditionalDownload> Files { get; set; }
    }

    public partial class Score
    {
        [JsonProperty("score", NullValueHandling = NullValueHandling.Ignore)]
        public double? ScoreScore { get; set; }

        [JsonProperty("votes", NullValueHandling = NullValueHandling.Ignore)]
        public long? Votes { get; set; }
    }

    public partial class Screen
    {
        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("scrUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string ScrUrl { get; set; }

        [JsonProperty("format")]
        public Format Format { get; set; }

        [JsonProperty("release_seq")]
        [JsonConverter(typeof(DecodingChoiceConverter))]
        public long ReleaseSeq { get; set; }

        [JsonProperty("type")]
        public ScreenType Type { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("entry_id")]
        public long EntryId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public partial class Total
    {
        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("relation")]
        public string Relation { get; set; }
    }

    public partial class Shards
    {
        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("failed")]
        public long Failed { get; set; }

        [JsonProperty("successful")]
        public long Successful { get; set; }

        [JsonProperty("skipped")]
        public long Skipped { get; set; }
    }

    public enum Country { Norway, Romania, Turkey, Uk };

    public enum GroupTypeEnum { Company, CompanyPublisherManager, CompanyUserGroup, Person };

    public enum LabelType { Nickname, Person };

    public enum AuthorType { Contributor, Creator };

    public enum Format { Picture, PictureGif };

    public enum ScreenType { LoadingScreen, OpeningScreen, RunningScreen };

    public partial struct Sort
    {
        public double? Double;
        public string String;

        public static implicit operator Sort(double Double) => new Sort { Double = Double };
        public static implicit operator Sort(string String) => new Sort { String = String };
    }

    public partial class InfoModel
    {
        public static InfoModel FromJson(string json) => JsonConvert.DeserializeObject<InfoModel>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this InfoModel self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                CountryConverter.Singleton,
                GroupTypeEnumConverter.Singleton,
                LabelTypeConverter.Singleton,
                AuthorTypeConverter.Singleton,
                FormatConverter.Singleton,
                ScreenTypeConverter.Singleton,
                SortConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class CountryConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Country) || t == typeof(Country?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Norway":
                    return Country.Norway;
                case "Romania":
                    return Country.Romania;
                case "Turkey":
                    return Country.Turkey;
                case "UK":
                    return Country.Uk;
            }
            throw new Exception("Cannot unmarshal type Country");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Country)untypedValue;
            switch (value)
            {
                case Country.Norway:
                    serializer.Serialize(writer, "Norway");
                    return;
                case Country.Romania:
                    serializer.Serialize(writer, "Romania");
                    return;
                case Country.Turkey:
                    serializer.Serialize(writer, "Turkey");
                    return;
                case Country.Uk:
                    serializer.Serialize(writer, "UK");
                    return;
            }
            throw new Exception("Cannot marshal type Country");
        }

        public static readonly CountryConverter Singleton = new CountryConverter();
    }

    internal class GroupTypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(GroupTypeEnum) || t == typeof(GroupTypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Company":
                    return GroupTypeEnum.Company;
                case "Company: Publisher/Manager":
                    return GroupTypeEnum.CompanyPublisherManager;
                case "Company: User group":
                    return GroupTypeEnum.CompanyUserGroup;
                case "Person":
                    return GroupTypeEnum.Person;
            }
            throw new Exception("Cannot unmarshal type GroupTypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (GroupTypeEnum)untypedValue;
            switch (value)
            {
                case GroupTypeEnum.Company:
                    serializer.Serialize(writer, "Company");
                    return;
                case GroupTypeEnum.CompanyPublisherManager:
                    serializer.Serialize(writer, "Company: Publisher/Manager");
                    return;
                case GroupTypeEnum.CompanyUserGroup:
                    serializer.Serialize(writer, "Company: User group");
                    return;
                case GroupTypeEnum.Person:
                    serializer.Serialize(writer, "Person");
                    return;
            }
            throw new Exception("Cannot marshal type GroupTypeEnum");
        }

        public static readonly GroupTypeEnumConverter Singleton = new GroupTypeEnumConverter();
    }

    internal class LabelTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(LabelType) || t == typeof(LabelType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Nickname":
                    return LabelType.Nickname;
                case "Person":
                    return LabelType.Person;
            }
            throw new Exception("Cannot unmarshal type LabelType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (LabelType)untypedValue;
            switch (value)
            {
                case LabelType.Nickname:
                    serializer.Serialize(writer, "Nickname");
                    return;
                case LabelType.Person:
                    serializer.Serialize(writer, "Person");
                    return;
            }
            throw new Exception("Cannot marshal type LabelType");
        }

        public static readonly LabelTypeConverter Singleton = new LabelTypeConverter();
    }

    internal class AuthorTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AuthorType) || t == typeof(AuthorType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Contributor":
                    return AuthorType.Contributor;
                case "Creator":
                    return AuthorType.Creator;
            }
            throw new Exception("Cannot unmarshal type AuthorType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (AuthorType)untypedValue;
            switch (value)
            {
                case AuthorType.Contributor:
                    serializer.Serialize(writer, "Contributor");
                    return;
                case AuthorType.Creator:
                    serializer.Serialize(writer, "Creator");
                    return;
            }
            throw new Exception("Cannot marshal type AuthorType");
        }

        public static readonly AuthorTypeConverter Singleton = new AuthorTypeConverter();
    }

    internal class FormatConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Format) || t == typeof(Format?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Picture":
                    return Format.Picture;
                case "Picture (GIF)":
                    return Format.PictureGif;
            }
            throw new Exception("Cannot unmarshal type Format");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Format)untypedValue;
            switch (value)
            {
                case Format.Picture:
                    serializer.Serialize(writer, "Picture");
                    return;
                case Format.PictureGif:
                    serializer.Serialize(writer, "Picture (GIF)");
                    return;
            }
            throw new Exception("Cannot marshal type Format");
        }

        public static readonly FormatConverter Singleton = new FormatConverter();
    }

    internal class DecodingChoiceConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return integerValue;
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    long l;
                    if (Int64.TryParse(stringValue, out l))
                    {
                        return l;
                    }
                    break;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value);
            return;
        }

        public static readonly DecodingChoiceConverter Singleton = new DecodingChoiceConverter();
    }

    internal class ScreenTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ScreenType) || t == typeof(ScreenType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Loading screen":
                    return ScreenType.LoadingScreen;
                case "Opening screen":
                    return ScreenType.OpeningScreen;
                case "Running screen":
                    return ScreenType.RunningScreen;
            }
            throw new Exception("Cannot unmarshal type ScreenType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ScreenType)untypedValue;
            switch (value)
            {
                case ScreenType.LoadingScreen:
                    serializer.Serialize(writer, "Loading screen");
                    return;
                case ScreenType.OpeningScreen:
                    serializer.Serialize(writer, "Opening screen");
                    return;
                case ScreenType.RunningScreen:
                    serializer.Serialize(writer, "Running screen");
                    return;
            }
            throw new Exception("Cannot marshal type ScreenType");
        }

        public static readonly ScreenTypeConverter Singleton = new ScreenTypeConverter();
    }

    internal class SortConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Sort) || t == typeof(Sort?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                case JsonToken.Float:
                    var doubleValue = serializer.Deserialize<double>(reader);
                    return new Sort { Double = doubleValue };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new Sort { String = stringValue };
            }
            throw new Exception("Cannot unmarshal type Sort");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Sort)untypedValue;
            if (value.Double != null)
            {
                serializer.Serialize(writer, value.Double.Value);
                return;
            }
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            throw new Exception("Cannot marshal type Sort");
        }

        public static readonly SortConverter Singleton = new SortConverter();
    }
}
