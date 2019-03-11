using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiLogic.Logics.SignUp.Model
{
    public class SignUpLeadDTO
    {
        public string Name { get; set; }
        /// <summary>
        /// Очное \ Дистанционное
        /// </summary>
        public string EducationForm { get; set; }
        /// <summary>
        /// Семинар \ Программа
        /// </summary>
        public string EducationType{ get; set; }
        public int Price { get; set; }
        public string Pay { get; set; }
        public DateTime Date { get; set; }
        /// <summary>
        /// Person data
        /// </summary>
        public bool Agree { get; set; }
        /// <summary>
        /// Guid from 1C service
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// Физ \ Юр лицо
        /// </summary>
        public bool? isPerson { get; set; } = null;
    }
}
