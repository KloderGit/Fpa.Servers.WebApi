using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Controllers.Listener.Models;
using WebApi.Infrastructure.Converters;

namespace WebApi.Controllers.Listener
{
    public class EventsModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var ResultObject = new List<EventViewModel>();

            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }
            var clueValue = bindingContext.HttpContext.Request.Form.Select(s => s.Key + "=" + s.Value);
            var urlParamsString = String.Join('&', clueValue);

            var json = new URLParamsToJsonConverter().Covert(urlParamsString) as JObject;
            var view = json.ToString();

            IList<string> entitiesNames = json.Properties().Select(p => p.Name).ToList();
            entitiesNames.Remove("account");
            entitiesNames.Remove("task");   // Задачи не обрабатываюся и нет моделей

            Dictionary<string, Type> TypeForEvent = new Dictionary<string, Type> {
                { "contacts", typeof(ContactViewModel) },
                { "leads", typeof(LeadViewModel) }
            };

            foreach (var entityName in entitiesNames)
            {
                var eventsNames = ( json[entityName] as JObject )
                                    .Properties().Select(p => p.Name).ToList();

                foreach (var eventName in eventsNames)
                {
                    var item = new EventViewModel();
                    item.Entity = entityName;
                    item.Event = eventName;
                    item.Entities = new List<EntityBase>();

                    foreach (var i in json[entityName][eventName])
                    {
                        var obj = ((JObject)i).ToObject(TypeForEvent[entityName]);

                        item.Entities.Add(obj as EntityBase);
                    }

                    ResultObject.Add(item);
                }

            }

            bindingContext.Result = ModelBindingResult.Success(ResultObject);

            return Task.CompletedTask;
        }
    }
}
