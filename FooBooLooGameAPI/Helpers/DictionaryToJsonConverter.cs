using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

public class DictionaryToJsonConverter : ValueConverter<Dictionary<int, string>, string>
{
    public DictionaryToJsonConverter() : base(
        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
        v => JsonSerializer.Deserialize<Dictionary<int, string>>(v, (JsonSerializerOptions)null))
    {
    }
}
