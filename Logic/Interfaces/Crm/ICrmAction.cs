using System;
using System.Collections.Generic;
using System.Text;
using WebApiLogic.Models.Crm;

namespace WebApiLogic.Interfaces.Crm
{
    public interface ICrmAction
    {
        void DoAction(object sender, CrmEvent e);
    }
}
