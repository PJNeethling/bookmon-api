using Bookmon.API.Models.ExceptionErrors;
using Newtonsoft.Json;

namespace Bookmon.API.Models.Responses;

public sealed class ErrorResponse
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "error")]
    public ExceptionDetails Error { get; set; }
}