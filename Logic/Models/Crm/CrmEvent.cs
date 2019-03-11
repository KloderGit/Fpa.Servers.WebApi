using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiLogic.Models.Crm
{
    public class CrmEvent
    {
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public List<ChangedParam> Events { get; set; }
        public string ContactType { get; set; }
        public string Account { get; set; }
    }
}
