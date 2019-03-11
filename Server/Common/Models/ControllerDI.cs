using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Common.Models
{
    public class ControllerDI : Controller
    {
        public static ILogger Logger { get; set; }
    }
}
