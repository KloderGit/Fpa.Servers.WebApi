using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Common.Models;
using WebApi.Infrastructure.Converters;

namespace WebApi.Common
{
    public class ListenedEventModelBinderApp : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {

            var ResultObject = new ListenedEvent();

            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }


            var ttt = bindingContext.HttpContext.Request.Form;

            var strFull = ttt.Select(s => s.Key + "=" + s.Value);

            var asd = String.Join('&', strFull);


            var result3 = new URLParamsToJsonConverter().Covert(asd);
            var view = result3.ToString();


            var names = bindingContext.HttpContext.Request.Form.Keys.FirstOrDefault().Split("[").Select(x => x.Replace("]", "")).ToArray();
            char[] charsToTrim = { '\n', '\r', ' ' };


            ResultObject.Entity = names[0].Trim(charsToTrim);

            string @event = names[1].Trim(charsToTrim);

            ResultObject.Events = GetEvents(ResultObject.Entity, @event, bindingContext);

            ResultObject.EntityId = bindingContext.ValueProvider.GetValue(ResultObject.Entity + "." + @event + "[0].id").FirstValue;

            ResultObject.ContactType = bindingContext.ValueProvider.GetValue(ResultObject.Entity + "." + @event + "[0].type").FirstValue;

            ResultObject.Account = bindingContext.ValueProvider.GetValue("account.subdomain").FirstValue;



            bindingContext.Result = ModelBindingResult.Success( ResultObject );
            
            return Task.CompletedTask;
        }

        List<ChangedParam> GetEvents(string entity, string @event, ModelBindingContext context)
        {
            var Events = new List<ChangedParam>();

            // Текущие виды параметров

            if (@event == "status")
            {
                var hasOldstatus = context.ValueProvider.GetValue(entity + "." + @event + "[0]" + "." + "old_status_id").FirstValue;

                if (!String.IsNullOrEmpty(hasOldstatus))
                {
                    var curentValue = context.ValueProvider.GetValue(entity + "." + @event + "[0]" + "." + "status_id").FirstValue;
                    var pipeline = context.ValueProvider.GetValue(entity + "." + @event + "[0]" + "." + "pipeline_id").FirstValue;

                    if (curentValue != hasOldstatus)
                    {
                        Events.Add(new ChangedParam { Event = @event, OldValue = hasOldstatus, CurrentValue = curentValue, Pipeline = pipeline });
                    }
                }
            }

            if (@event == "responsible")
            {
                var hasResponsibleUser = context.ValueProvider.GetValue(entity + "." + @event + "[0]" + "." + "old_responsible_user_id").FirstValue;

                if (!String.IsNullOrEmpty(hasResponsibleUser))
                {
                    var curentValue = context.ValueProvider.GetValue(entity + "." + @event + "[0]" + "." + "responsible_user_id").FirstValue;
                    if (curentValue != hasResponsibleUser)
                    {
                        Events.Add(new ChangedParam { Event = @event, OldValue = hasResponsibleUser, CurrentValue = curentValue });
                    }
                }
            }            

            if (Events.Count < 1)
            {
                Events.Add(new ChangedParam { Event = @event });
            }

            return Events;
        }
    }
}
