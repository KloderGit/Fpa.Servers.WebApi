using System.Collections.Generic;

namespace WebApi.Controllers.Listener.Models
{
    public class EventViewModel
    {
        public string Event { get; set; }
        public string Entity { get; set; }
        public List<EntityBase> Entities { get; set; }
    }
}
