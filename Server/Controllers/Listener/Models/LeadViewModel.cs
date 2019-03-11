using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebApi.Controllers.Listener.Models
{
    public class LeadViewModel : EntityBase
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "modified_user_id")]
        public int ModifiedUserId { get; set; }

        [JsonProperty(PropertyName = "status_id")]
        public int? Status { get; set; }

        [JsonProperty(PropertyName = "old_status_id")]
        public int OldStatusId { get; set; }

        [JsonProperty(PropertyName = "price")]
        public int? Price { get; set; }

        [JsonProperty(PropertyName = "pipeline_id")]
        public int? PipelineId { get; set; }

        [JsonProperty(PropertyName = "custom_fields")]
        public IEnumerable<CustomFieldViewModel> CustomFields { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public IEnumerable<TagViewModel> Tags { get; set; }
    }
}
