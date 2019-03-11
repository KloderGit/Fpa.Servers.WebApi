using Mapster;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace WebApi.Utils
{
    public class RegisterMapsterConfig
    {
        public RegisterMapsterConfig()
        {
            Assembly webApi = typeof(ToDTO).GetTypeInfo().Assembly;

            TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);
            TypeAdapterConfig.GlobalSettings.Scan(webApi);
        }
    }
}
