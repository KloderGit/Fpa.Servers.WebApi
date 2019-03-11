using Newtonsoft.Json;

namespace WebApi.Controllers.Listener.Models
{
    public class LinkedLeadsViewModel
    {
        [JsonProperty(PropertyName = "ID")]
        public int Id { get; set; }
    }
}
