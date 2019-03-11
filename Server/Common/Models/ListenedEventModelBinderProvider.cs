using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Common.Models
{
    public class ListenedEventModelBinderProvider : IModelBinderProvider
    {
        private readonly IModelBinder binder = new ListenedEventModelBinderApp();

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return context.Metadata.ModelType == typeof(ListenedEvent) ? binder : null;
        }
    }
}
