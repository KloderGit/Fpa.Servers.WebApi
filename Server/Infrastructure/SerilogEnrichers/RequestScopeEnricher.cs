using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Common.Models;

namespace WebApi.Infrastructure.SerilogEnrichers
{
    public class RequestScopeEnricher : ILogEventEnricher
    {
        RequestScope requestScope;

        public RequestScopeEnricher(RequestScope requestScope)
        {
            this.requestScope = requestScope;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(
                    "~RequestScope", requestScope.Guid));
        }
    }
}
