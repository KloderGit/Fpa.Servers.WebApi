using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApi.Common.Models
{
    [ModelBinder(typeof(ListenedEventModelBinderApp))]
    public class ListenedEvent
    {
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public List<ChangedParam> Events { get; set; }
        public string ContactType { get; set; }
        public string Account { get; set; }
    }
}
