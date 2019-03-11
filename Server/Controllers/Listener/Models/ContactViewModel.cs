using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebApi.Controllers.Listener.Models
{
    public class ContactViewModel : EntityBase
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "modified_user_id")]
        public int UpdatedBy { get; set; }

        [JsonProperty(PropertyName = "custom_fields")]
        public IEnumerable<CustomFieldViewModel> CustomFields { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public IEnumerable<TagViewModel> Tags { get; set; }

        [JsonProperty(PropertyName = "linked_leads_id")]
        public IEnumerable<LinkedLeadsViewModel> LinkedLeads { get; set; }
    }
}
