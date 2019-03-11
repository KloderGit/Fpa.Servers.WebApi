using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Common.Models
{
    //public class EntityValueProvider : IValueProvider
    //{
    //    public bool ContainsPrefix(string prefix)
    //    {
    //        var array = Enum.GetNames(typeof(EntityEnums));
    //        var result = array.Contains(prefix);

    //        return result;
    //    }

    //    public ValueProviderResult GetValue(string key)
    //    {
    //        if (ContainsPrefix(key))
    //        {
    //            return new ValueProviderResult(key, CultureInfo.InvariantCulture);
    //        }
    //        else
    //        {
    //            return new ValueProviderResult();
    //        }            
    //    }
    //}
}
