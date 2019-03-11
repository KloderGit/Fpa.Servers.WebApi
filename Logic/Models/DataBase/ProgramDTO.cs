using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPortalBuisenessLogic.Models.DataBase
{
    public class ProgramDTO
    {
        public string Guid { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Variant { get; set; }
        public string Form { get; set; }
        public string Department { get; set; }
        public IEnumerable<string> Subjects { get; set; }
    }
}
