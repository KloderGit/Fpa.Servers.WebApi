using Mapster;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace WebApiLogic.Utils.Mapster
{
    public class RegisterMapsterConfig
    {
        public RegisterMapsterConfig()
        {
            Assembly webApiAmoCrm = typeof(WebApiAmoCrm).GetTypeInfo().Assembly;

            TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);
            TypeAdapterConfig.GlobalSettings.Scan(webApiAmoCrm);
        }
    }
}
