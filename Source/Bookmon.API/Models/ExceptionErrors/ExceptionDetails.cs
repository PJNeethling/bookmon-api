using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bookmon.API.Models.ExceptionErrors;

public sealed class ExceptionDetails
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "code")]
    public int? Code { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "message")]
    public string Message { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type")]
    public string Type { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "errors")]
    public List<ErrorDetails> Errors { get; set; }
}