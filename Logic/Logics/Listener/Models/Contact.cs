using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiLogic.Logics.Listener.Models
{
    public class Contact : Domain.Models.Crm.Contact
    {
        public int OldResponsibleUserId { get; set; }
    }
}
