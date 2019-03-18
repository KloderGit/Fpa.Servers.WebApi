using Domain.Models.Crm;
using Domain.Models.Crm.Parent;
using System;
using WebApiLogic.Logics.Listener.DTO;
using WebApiLogic.Logics.Listener.EventHandlers;

namespace WebApiLogic.Logics.Listener
{
    public class CrmEventHandler
    {
        public event EventHandler<EventDTO> Add;
        public event EventHandler<EventDTO> Update;
        public event EventHandler<EventDTO> Responsible;
        public event EventHandler<EventDTO> Delete;
        public event EventHandler<EventDTO> Note;
        public event EventHandler<EventDTO> Status;

        public virtual void OnAdd(EventDTO e) { Add?.Invoke(this, e); }

        public virtual void OnUpdate(EventDTO e) { Update?.Invoke(this, e); }

        public virtual void OnResponsible(EventDTO e) { Responsible?.Invoke(this, e); }

        public virtual void OnDelete(EventDTO e) { Delete?.Invoke(this, e); }

        public virtual void OnNote(EventDTO e) { Note?.Invoke(this, e); }

        public virtual void OnStatus(EventDTO e) { Status?.Invoke(this, e); }


    }

    public class toUpdate
    {
        Func<EntityCore, EntityCore> func = null;

        //Func<TIn, TOut> func = null;

        public void toDo(object sender, EventDTO e)
        {
            foreach (var item in e.Entities)
            {
                var result = func(item);
            }
        }
    }

}
