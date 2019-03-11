using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using WebApi.Controllers.Listener.Models;

namespace WebApi.Controllers.Listener
{
    internal class FieldObjectJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object retVal = new Object();

            if (reader.TokenType == JsonToken.StartArray)
            {
                retVal = serializer.Deserialize(reader, objectType);
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                var df = serializer.Deserialize(reader, typeof(JObject));
                var oob = new FieldViewModel() { Value = ((JObject)df)["0"].ToString() };
                var lst = new List<FieldViewModel>();
                lst.Add(oob);
                retVal = lst;
            }
            return retVal;
        }

        public override bool CanConvert(Type objectType)
        {
            return false;
        }
    }
}
