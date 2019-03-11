using System;
using System.Collections.Generic;
using System.Text;
using WebApiLogic.Infrastructure.CrmDoEventActions;

namespace WebApiLogic.Models.Crm
{
    public class CrmEventTypes
    {
        public event EventHandler<CrmEvent> Add;
        public event EventHandler<CrmEvent> Update;
        public event EventHandler<CrmEvent> Responsible;
        public event EventHandler<CrmEvent> Delete;
        public event EventHandler<CrmEvent> Note;
        public event EventHandler<CrmEvent> Status;

        public virtual void OnAdd(CrmEvent e) { Add?.Invoke(this, e); }

        public virtual void OnUpdate(CrmEvent e) { Update?.Invoke(this, e); }

        public virtual void OnResponsible(CrmEvent e) { Responsible?.Invoke(this, e); }

        public virtual void OnDelete(CrmEvent e) { Delete?.Invoke(this, e); }

        public virtual void OnNote(CrmEvent e) { Note?.Invoke(this, e); }

        public virtual void OnStatus(CrmEvent e) { Status?.Invoke(this, e); }
    }
}
