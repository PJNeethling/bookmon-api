using System.Text;

namespace Bookmon.Domain.Utility;

public static class Serializers
{
    public static byte[] SerializeThroughJson(this object target)
    {
        ArgumentNullException.ThrowIfNull(target);

        var jsonString = System.Text.Json.JsonSerializer.Serialize(target);
        var encodedBytes = Encoding.UTF8.GetBytes(jsonString);

        return encodedBytes;
    }

    public static T DeserializeThroughJson<T>(this byte[] buffer)
    {
        var jsonString = Encoding.UTF8.GetString(buffer);

        return System.Text.Json.JsonSerializer.Deserialize<T>(jsonString);
    }
}