using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Common.Models
{
    public class RequestScope
    {
        public Guid Guid { get; private set; } = Guid.NewGuid();
    }
}
