using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Master.Storage.Events
{
    public class StoreMaterialEventHandler :
       IEventHandler<EntityChangedEventData<StoreMaterial>>,

        ITransientDependency
    {
 
        public StoreMaterialHistoryManager StoreMaterialHistoryManager { get; set; }
        public StoreMaterialManager StoreMaterialManager { get; set; }
        public MaterialManager MaterialManager { get; set; }
        /// <summary>
        /// 库存变动明细
        /// </summary>
        /// <param name="eventData"></param>
        [UnitOfWork]
        public virtual void HandleEvent(EntityChangedEventData<StoreMaterial> eventData)
        {
            var entity = eventData.Entity;
            var dbContext = StoreMaterialManager.Repository.GetDbContext();
            dbContext.Attach(entity);
            //var entity = StoreMaterialManager.GetByIdAsync(newEntity.Id).GetAwaiter().GetResult();

            StoreMaterialManager.Repository.EnsurePropertyLoaded(entity, o => o.Store);
            StoreMaterialManager.Repository.EnsurePropertyLoaded(entity, o => o.Material);

            var storeMaterialHistory = new StoreMaterialHistory()
                {
                    MaterialId = entity.MaterialId,
                    StoreId = entity.StoreId,
                    MaterialName = entity.Material.Name,
                    Specification = entity.Material.Specification,
                    StoreName = entity.Store?.Name,
                    MeasureMentUnitName = entity.Material.MeasureMentUnit,
                    ChangeType = entity.StoreMaterialHistory.ChangeType,
                    Number = entity.StoreMaterialHistory.Number,
                    FlowSheetId = entity.StoreMaterialHistory.FlowSheetId,
                };
 

            StoreMaterialHistoryManager.InsertAsync(storeMaterialHistory).GetAwaiter().GetResult();
        }
    }
}
