using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Infrastructure.ModelBinders;

namespace WebApi.Models
{
    public class IncomingLeadViewModel
    {
        public IncomingLeadViewModel(){}

        public string ContactName { get; set; }

        public IEnumerable<string> ContactPhones { get; set; }

        public IEnumerable<string> ContactEmails { get; set; }

        public string ContactCity { get; set; }

        public string LeadName { get; set; }

        public DateTime? LeadDate { get; set; }

        public string LeadType { get; set; }

        public string EventType { get; set; }

        public int LeadPrice { get; set; }

        public string LeadGuid { get; set; }
    }

    public class LeadFormViewModel
    {
        public IEnumerable<FormField> Fields { get; set; }
    }

    public class FormField
    {
        public FormField() {}

        public string name { get; set; }

        public string value { get; set; }
    }
}
