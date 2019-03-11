using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Infrastructure.ModelBinders
{
    public class IncomingLeadModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var model = new LeadFormViewModel();

            if (bindingContext == null)
            {
                throw new ArgumentNullException( nameof( bindingContext ) );
            }
            var reader = new StreamReader( bindingContext.HttpContext.Request.Body ).ReadToEnd();
            var json = JArray.Parse( reader );


            return Task.CompletedTask;
        }
    }
}
