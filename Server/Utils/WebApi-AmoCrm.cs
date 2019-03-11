using Domain.Models.Crm;
using LibraryAmoCRM.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.Utils
{
    public class ToDTO : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<WebApi.Common.Models.ListenedEvent, WebApiBusinessLogic.Models.Crm.CrmEvent>();

        }
    }
}
