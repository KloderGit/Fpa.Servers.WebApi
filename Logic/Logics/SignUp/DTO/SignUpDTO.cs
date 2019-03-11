using Domain.Models.Crm;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiLogic.Logics.SignUp.Model
{
    public class SignUpDTO
    {
        public SignUpContactDTO Contact { get; set; }
        public SignUpLeadDTO Lead { get; set; }

        public string Source { get; set; }
    }
}
