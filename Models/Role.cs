using System.Text.Json.Serialization;

namespace NWI.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        user = 2,
        admin = 1,
        
    }
}