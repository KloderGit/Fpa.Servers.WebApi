using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiLogic.Logics.Listener.DTO;

namespace WebApiLogic.Logics.Listener.EventHandlers
{
    public interface IDoHandlerAction
    {
        void DoActionAsync(object sender, EventDTO events);
    }
}
