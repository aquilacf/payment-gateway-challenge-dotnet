using System.Text.Json;
using System.Text.Json.Serialization;

namespace PaymentGateway.Domain;

public static class Constants
{
    public static JsonSerializerOptions DefaultJsonSerializerOptions => new()
    {
        Converters = { new JsonStringEnumConverter() },
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public static void ApplyDefaultSettings(JsonSerializerOptions options)
    {
        options.PropertyNamingPolicy = DefaultJsonSerializerOptions.PropertyNamingPolicy;
        options.DictionaryKeyPolicy = DefaultJsonSerializerOptions.DictionaryKeyPolicy;
        options.DefaultIgnoreCondition = DefaultJsonSerializerOptions.DefaultIgnoreCondition;

        foreach (var converter in DefaultJsonSerializerOptions.Converters)
        {
            options.Converters.Add(converter);
        }
    }
}