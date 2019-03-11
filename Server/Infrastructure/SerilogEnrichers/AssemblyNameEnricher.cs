using Serilog.Core;
using Serilog.Events;
using System.Linq;

namespace WebApi.Infrastructure.SerilogEnrichers
{
    public class AssemblyNameEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                    "~Assembly", Name(logEvent)));

            string Name(LogEvent data)
            {
                string result = "";

                var sourceContext = data.Properties.FirstOrDefault(k => k.Key == "SourceContext");
                if (sourceContext.Key != null)
                {
                    result = sourceContext.Value.ToString().Trim('\"').Split('.').FirstOrDefault();
                }

                return result;
            }
        }
    }
}
