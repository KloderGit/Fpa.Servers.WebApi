using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiLogic.Logics.Listener.DTO;

namespace WebApiLogic.Logics.Listener.EventHandlers
{
    public interface IDoAction<T>
    {
        Task<T> DoActionAsync(T item);
    }
}
