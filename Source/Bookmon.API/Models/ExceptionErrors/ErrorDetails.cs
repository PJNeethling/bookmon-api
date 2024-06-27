using Newtonsoft.Json;

namespace Bookmon.API.Models.ExceptionErrors;

public sealed class ErrorDetails
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "reason")]
    public string Reason { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "message")]
    public string Message { get; set; }
}