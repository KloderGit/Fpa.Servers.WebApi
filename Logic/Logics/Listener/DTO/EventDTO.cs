using Domain.Interfaces;
using Domain.Models.Crm.Parent;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiLogic.Logics.Listener.DTO
{
    public class EventDTO
    {
        public string Event { get; set; }
        public string Entity { get; set; }
        public IEnumerable<EntityCore> Entities { get; set; }
    }
}
