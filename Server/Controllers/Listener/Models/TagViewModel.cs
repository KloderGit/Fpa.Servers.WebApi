using Newtonsoft.Json;

namespace WebApi.Controllers.Listener.Models
{
    public class TagViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public int? Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
