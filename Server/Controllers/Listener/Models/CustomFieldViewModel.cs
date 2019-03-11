using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebApi.Controllers.Listener.Models
{
    public class CustomFieldViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonConverter(typeof(FieldObjectJsonConverter))]
        [JsonProperty(PropertyName = "values")]
        public IEnumerable<FieldViewModel> Values { get; set; }
    }
}
