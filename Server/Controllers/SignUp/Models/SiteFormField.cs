using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers.SignUp.Models
{
    public class SiteFormField
    {
        [JsonProperty( PropertyName = "name" )]
        public string Name { get; set; }

        [JsonProperty( PropertyName = "value" )]
        public string Value { get; set; }
    }
}
