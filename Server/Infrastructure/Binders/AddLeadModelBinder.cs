using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Common.Models;
using WebApi.Controllers.SignUp.Models;

namespace WebApi.Infrastructure.Binders
{
    public class AddLeadModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var jsonStringData = new StreamReader( bindingContext.HttpContext.Request.Body ).ReadToEnd();

            var result = JsonConvert.DeserializeObject<IEnumerable<SiteFormField>>( jsonStringData );

            bindingContext.Result = ModelBindingResult.Success( result );

            return Task.CompletedTask;
        }
    }
}