using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace api.src.Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Activity
    {
        Jump,
        Sit,
        PushUp
    }
}