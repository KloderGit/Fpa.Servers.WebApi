using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiLogic.Logics.Listener.Models
{
    public class Lead : Domain.Models.Crm.Lead
    {
        public int OldStatusId { get; set; }
        public int OldResponsibleUserId { get; set; }
    }
}
