using Newtonsoft.Json;

namespace WebApi.Controllers.Listener.Models
{
    public class FieldViewModel
    {
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "enum")]
        public int? @Enum { get; set; }
    }
}
