using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.Storage.Events
{
    public class StoreEventHandler : IEventHandler<EntityChangedEventData<Store>>,
        ITransientDependency
    {
        public StoreManager StoreManager { get; set; }
        [UnitOfWork]
        public virtual void HandleEvent(EntityChangedEventData<Store> eventData)
        {
            //如果仓库设置为默认了，则设置其余仓库为非默认
            if (eventData.Entity.IsDefault)
            {
                var otherStores = StoreManager.GetAll().Where(o => o.Id != eventData.Entity.Id).ToList();
                foreach(var store in otherStores)
                {
                    store.IsDefault = false;
                }
            }
        }
    }
}
