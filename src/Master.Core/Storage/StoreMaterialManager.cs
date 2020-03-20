using Master.Domain;
using Master.Entity;
using Master.Module;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Abp.UI;
using Master.WorkFlow;

namespace Master.Storage
{
    public class StoreMaterialManager : ModuleServiceBase<StoreMaterial, int>
    {
        #region 库存是否足够
        /// <summary>
        /// 出库数量是否足够
        /// </summary>
        /// <param name="materialId"></param>
        /// <param name="storeId"></param>
        /// <param name="unitId"></param>
        /// <param name="outNumber"></param>
        /// <returns></returns>
        public virtual async Task<bool> IsSatisfied(int materialId,int storeId,int unitId,decimal outNumber)
        {
            var materialManager = Resolve<MaterialManager>();
            var material = await materialManager.GetByIdAsync(materialId);
            var unitSellMode = await materialManager.GetMaterialUnitSellMode(material, unitId);
            if (unitSellMode == UnitSellMode.停止销售)
            {
                return false;
            }else if (unitSellMode == UnitSellMode.始终销售)
            {
                return true;
            }
            else
            {
                var storeMaterialCount = await GetStoreMaterialNumber(storeId, materialId);
                return storeMaterialCount > outNumber;
            }
        }
        #endregion

        #region 商品库存
        /// <summary>
        /// 获取商品在某个仓库的库存
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public virtual async Task<decimal> GetStoreMaterialNumber(int storeId, int materialId)
        {
            return (await GetAll().Where(o => o.StoreId == storeId && o.MaterialId == materialId).FirstOrDefaultAsync())?.Number ?? 0;
        }
        /// <summary>
        /// 获取商品库存
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public virtual async Task<decimal> GetMaterialNumber(int materialId)
        {
            return await GetAll().Where(o => o.MaterialId == materialId).SumAsync(o => o.Number);
        } 
        #endregion

        #region 入库
        public virtual async Task InMaterial(int storeId, int materialId, int number, FlowSheet flowSheet)
        {
            var changeType = flowSheet.ChangeType;
            var materialManager = Resolve<MaterialManager>();
            var storeMaterial = new StoreMaterial()
            {
                MaterialId = materialId,
                StoreId = storeId,
                Number = number
            };

            storeMaterial.BuildStoreMaterialHistory(changeType, number, flowSheet.Id);
            await AppendStoreMaterial(storeMaterial);
        }
        #endregion

        #region 拆散与组合
        /// <summary>
        /// 组合商品
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="materialId"></param>
        /// <param name="number"></param>
        /// <param name="flowSheetId"></param>
        /// <returns></returns>
        public virtual async Task CombineMaterial(int storeId, int materialId, int number, FlowSheet flowSheet)
        {
            var changeType = flowSheet.ChangeType;
            var materialManager = Resolve<MaterialManager>();
            var material = await materialManager.GetByIdAsync(materialId);
            if (!material.IsDiyed || material.DIYInfo.Count == 0)
            {
                throw new UserFriendlyException($"商品{material.Name}未配置组合信息,无法组合");
            }
            //1.减少单品库存
            foreach (var diyInfo in material.DIYInfo)
            {
                //是否有足够库存
                var diyMaterialNumber = await GetStoreMaterialNumber(storeId, diyInfo.MaterialId);
                if(diyMaterialNumber< diyInfo.Number * number)
                {
                    var diyMaterial = await materialManager.GetByIdFromCacheAsync(diyInfo.MaterialId);
                    throw new UserFriendlyException($"商品{diyMaterial.Name}需要数量{diyInfo.Number * number}才能装配目标商品,目前库存数量为{diyMaterialNumber.ToString("0")},无法装配");
                }
                var diyStoreMaterial = new StoreMaterial()
                {
                    MaterialId = diyInfo.MaterialId,
                    StoreId = storeId,
                    Number = -diyInfo.Number * number
                };
                
                diyStoreMaterial.BuildStoreMaterialHistory(changeType, -diyInfo.Number * number, flowSheet.Id);
                await AppendStoreMaterial(diyStoreMaterial);
            }
            //2.增加组装库存
            var storeMaterial = new StoreMaterial()
            {
                MaterialId = materialId,
                StoreId = storeId,
                Number = number
            };
            storeMaterial.BuildStoreMaterialHistory(changeType, number, flowSheet.Id);
            await AppendStoreMaterial(storeMaterial);
            
        }
        /// <summary>
        /// 拆散商品
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public virtual async Task BreakMaterial(int storeId, int materialId, int number, FlowSheet flowSheet)
        {
            var changeType = flowSheet.ChangeType;
            var material = await Resolve<MaterialManager>().GetByIdAsync(materialId);
            if (!material.IsDiyed || material.DIYInfo.Count == 0)
            {
                throw new UserFriendlyException($"商品{material.Name}未配置组合信息,无法拆散");
            }
            var storeMaterialCount = await GetStoreMaterialNumber(storeId, materialId);
            if (storeMaterialCount < number)
            {
                throw new UserFriendlyException("拆散数量不能大于库存数量");
            }

            //1.减少组装库存
            var storeMaterial = new StoreMaterial()
            {
                MaterialId = materialId,
                StoreId = storeId,
                Number = -number
            };
            storeMaterial.BuildStoreMaterialHistory(changeType, -number, flowSheet.Id);
            await AppendStoreMaterial(storeMaterial);
            //2.增加单品库存
            foreach (var diyInfo in material.DIYInfo)
            {
                var diyStoreMaterial = new StoreMaterial()
                {
                    MaterialId = diyInfo.MaterialId,
                    StoreId = storeId,
                    Number = diyInfo.Number * number
                };
                diyStoreMaterial.BuildStoreMaterialHistory(changeType, diyInfo.Number * number, flowSheet.Id);
                await AppendStoreMaterial(diyStoreMaterial);
            }
        }
        #endregion

        #region 调拨
        /// <summary>
        /// 仓库调拨
        /// </summary>
        /// <param name="outStoreId"></param>
        /// <param name="inStoreId"></param>
        /// <param name="materialId"></param>
        /// <param name="number"></param>
        /// <param name="flowSheet"></param>
        /// <returns></returns>
        public virtual async Task TransferMaterial(int outStoreId,int inStoreId,int materialId, int number, FlowSheet flowSheet)
        {
            var changeType = flowSheet.ChangeType;
            var material = await Resolve<MaterialManager>().GetByIdAsync(materialId);
           
            var storeMaterialCount = await GetStoreMaterialNumber(outStoreId, materialId);
            if (storeMaterialCount < number)
            {
                throw new UserFriendlyException("调拨数量不能大于库存数量");
            }

            //1.减少调出仓库库存
            var outStoreMaterial = new StoreMaterial()
            {
                MaterialId = materialId,
                StoreId = outStoreId,
                Number = -number
            };
            outStoreMaterial.BuildStoreMaterialHistory(changeType, -number, flowSheet.Id);
            await AppendStoreMaterial(outStoreMaterial);
            //2.增加调入仓库库存
            var inStoreMaterial = new StoreMaterial()
            {
                MaterialId = materialId,
                StoreId = inStoreId,
                Number = number
            };
            inStoreMaterial.BuildStoreMaterialHistory(changeType, number, flowSheet.Id);
            await AppendStoreMaterial(inStoreMaterial);
        }
        #endregion

        #region 库存变化
        public virtual async Task CountMaterial(int storeId, int materialId, int number, FlowSheet flowSheet)
        {
            var changeType = flowSheet.ChangeType;
            var materialManager = Resolve<MaterialManager>();
            //var material = await materialManager.GetByIdAsync(materialId);
            var storeMaterial = new StoreMaterial()
            {
                MaterialId = materialId,
                StoreId = storeId,
                Number = number
            };

            storeMaterial.BuildStoreMaterialHistory(changeType, number, flowSheet.Id);
            await AppendStoreMaterial(storeMaterial);
        }
        #endregion

        //   public MaterialUseInfoManager MaterialUseInfoManager { get; set; }
        //   public StoreMaterialHistoryManager StoreMaterialHistoryManager { get; set; }
        //  public FlowSheetManager FlowSheetManager { get; set; }
        /// <summary>
        /// 增加库存信息
        /// </summary>
        /// <param name="storeMaterial"></param>

        public virtual async Task<StoreMaterial> AppendStoreMaterial(StoreMaterial storeMaterial)
        {
            var material = await Resolve<MaterialManager>().GetByIdAsync(storeMaterial.MaterialId);
            material.TotalNumber += storeMaterial.Number;
            var oriStoreMaterial = await FindExistStoreMaterialRecord(storeMaterial);
            if (oriStoreMaterial == null)
            {
                //如果没有记录则产生库存记录
                await InsertAsync(storeMaterial);
                await CurrentUnitOfWork.SaveChangesAsync();
                return storeMaterial;
            }
            else
            {
                //如果已经有记录了,则更新原有记录
                oriStoreMaterial.Number += storeMaterial.Number;//更新库存数量
                oriStoreMaterial.StoreMaterialHistory = storeMaterial.StoreMaterialHistory;
                await CurrentUnitOfWork.SaveChangesAsync();
                return oriStoreMaterial;
            }
        }

        /// <summary>
        /// 寻找库存中已存在的库存记录
        /// </summary>
        /// <param name="storeMaterial"></param>
        /// <returns></returns>
        public virtual async Task<StoreMaterial> FindExistStoreMaterialRecord(StoreMaterial storeMaterial)
        {
            return await GetAll()
                .Where(o => o.MaterialId == storeMaterial.MaterialId)
                .Where(o => o.StoreId == storeMaterial.StoreId)
                .SingleOrDefaultAsync();
        }

        ///// <summary>
        ///// 库存出库
        ///// </summary>
        ///// <param name="storeMaterialId"></param>
        ///// <param name="number"></param>
        ///// <param name="useDate"></param>
        ///// <param name="usePerson"></param>
        ///// <param name="projectId"></param>
        ///// <param name="materialRequireId"></param>
        ///// <param name="flowSheetId"></param>
        ///// <returns></returns>
        //public virtual async Task OutStoreMaterial(int storeMaterialId,decimal number,DateTime useDate,string usePerson, int? projectId,int?materialRequireId, int? flowSheetId)
        //{
        //    //对应的工作流程
        //    FlowInstance flowInstance = null;
        //    if (flowSheetId.HasValue)
        //    {
        //        var flowSheet=await Resolve<FlowSheetManager>().GetAll()
        //            .Include(o => o.FlowInstance).ThenInclude(o=>o.FlowForm)
        //            .Where(o => o.Id == flowSheetId.Value)
        //            .SingleOrDefaultAsync();
        //        flowInstance = flowSheet?.FlowInstance;
        //    }
        //    //更新库存
        //    var storeMaterial = await GetByIdAsync(storeMaterialId);
        //    storeMaterial.Number -= number;
        //    //产生领用明细
        //    var materialUseInfo = new MaterialUseInfo()
        //    {
        //        MaterialId = storeMaterial.MaterialId,
        //        ProjectId = projectId,
        //        UnitId = storeMaterial.UnitId,
        //        CustomName = storeMaterial.CustomName,
        //        CustomSpecification = storeMaterial.CustomSpecification,
        //        CustomBrand = storeMaterial.CustomBrand,
        //        Batch = storeMaterial.Batch,
        //        MeasureMentUnitName = storeMaterial.MeasureMentUnit.Name,
        //        Number = number,
        //        Price = storeMaterial.Price,
        //        Cost =Math.Round( storeMaterial.Price*number,2),
        //        UseDate = useDate,
        //        UsePerson = usePerson,
        //        UseSource = UseSource.Storage,
        //        FlowSheetId = flowSheetId,
        //        //CreatorUserId = instance.CreatorUserId
        //    };
        //    //如果是流程过来的,则取流程的发起人作为领用记录的创建人
        //    if (flowInstance != null) { materialUseInfo.CreatorUserId = flowInstance.CreatorUserId; }
        //    //将对应的物料需求Id保存至属性
        //    if (materialRequireId.HasValue)
        //    {
        //        materialUseInfo.SetPropertyValue("MaterialRequireId", materialRequireId.Value);
        //    }            
        //    await Resolve<MaterialUseInfoManager>().InsertAsync(materialUseInfo);
        //    //产生出入库明细
        //    storeMaterial.BuildStoreMaterialHistory(flowInstance?.FlowForm.FormKey,-number, flowSheetId);

        //    //storeMaterialHistory.ChangeType = flowInstance?.FlowForm.FormKey;
        //    //storeMaterialHistory.Number = -number;
        //    //storeMaterialHistory.FlowSheetId = flowSheetId;
        //   // await StoreMaterialHistoryManager.InsertAsync(storeMaterialHistory);
        //}
        ///// <summary>
        ///// 库存拆包   /   
        ///// StoreMaterial.Number 及 StoreMaterial.Id 为需要拆分的信息     StoreMaterial.MeasureMentUnitId  为拆分后的单位
        ///// </summary>
        ///// <param name="StoreMaterial"></param>
        ///// <returns></returns>
        //public virtual async Task SplitStoreMaterial(StoreMaterialSplitDto StoreMaterialSplitDto) {
        //    var StoreMaterialFirst = await GetByIdAsync(StoreMaterialSplitDto.Id);

        //    // var str = JsonConvert.SerializeObject(StoreMaterialFirst);
        //    // var newStoreMaterial = JsonConvert.DeserializeObject<StoreMaterial>(str); 

        //  //  newStoreMaterial.MeasureMentUnitId = StoreMaterialSplitDto.MeasureMentUnitId;//拆分后的库存对象

        //    var MeasureMent = StoreMaterialFirst.MeasureMentUnit.MeasureMent;
        //    var firstUnit= MeasureMent.MeasureMentUnits.Where(o => o.Id == StoreMaterialFirst.MeasureMentUnitId).FirstOrDefault();//拆分前的单位 
        //    var lastUnit = MeasureMent.MeasureMentUnits.Where(o => o.Id == StoreMaterialSplitDto.MeasureMentUnitId).FirstOrDefault();//拆分后的单位

        //    StoreMaterialFirst.Number -= StoreMaterialSplitDto.Number;
        //    StoreMaterialFirst.BuildStoreMaterialHistory(firstUnit .Name+ "被拆分"+ lastUnit.Name, -StoreMaterialSplitDto.Number, null);

        //    var number = StoreMaterialSplitDto.SplitNumber;//直接记录   //自动换算//StoreMaterial.Number * lastRatio / firstRatio;//拆分后的数据


        //    //   newStoreMaterial.BuildStoreMaterialHistory(firstUnit.Name + "拆分成" + lastUnit.Name, StoreMaterialSplitDto.SplitNumber, null);

        //    var newStoreMaterial = new StoreMaterial
        //    {
        //        MeasureMentUnitId = StoreMaterialSplitDto.MeasureMentUnitId,
        //        Number = StoreMaterialSplitDto.SplitNumber,

        //        ProjectId = StoreMaterialFirst.ProjectId,
        //        MaterialId = StoreMaterialFirst.MaterialId,
        //        UnitId = StoreMaterialFirst.UnitId,
        //        StoreId = StoreMaterialFirst.StoreId,
        //        StoreLocationId = StoreMaterialFirst.StoreLocationId,
        //        CustomName = StoreMaterialFirst.CustomName,
        //        CustomSpecification = StoreMaterialFirst.CustomSpecification,
        //        CustomBrand = StoreMaterialFirst.CustomBrand,
        //        Batch = StoreMaterialFirst.Batch,

        //    };
        //    var storeMaterial = await FindExistStoreMaterialRecord(newStoreMaterial);
        //    if (storeMaterial == null) {
        //        storeMaterial = newStoreMaterial; 
        //        storeMaterial.BuildStoreMaterialHistory(firstUnit.Name + "拆分成" + lastUnit.Name, StoreMaterialSplitDto.SplitNumber, null);
        //        await InsertAsync(storeMaterial);
        //    }
        //    else {
        //        storeMaterial.BuildStoreMaterialHistory(firstUnit.Name + "拆分成" + lastUnit.Name, StoreMaterialSplitDto.SplitNumber, null);
        //        storeMaterial.Number += StoreMaterialSplitDto.SplitNumber;
        //    }


        //}
        ///// <summary>
        ///// 分页实体数据返回，判断是否是可拆分的多计量单位
        ///// </summary>
        ///// <param name="data"></param>
        ///// <param name="moduleInfo"></param>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public override Task FillEntityDataAfter(IDictionary<string, object> data, ModuleInfo moduleInfo, object entity)
        //{
        //    var id = Convert.ToInt32(data["Id"]);
        //    //var StoreMaterial =  GetByIdAsync(id).GetAwaiter().GetResult();
        //    var StoreMaterial = entity as StoreMaterial;

        //    var MeasureMent = StoreMaterial.MeasureMentUnit?.MeasureMent;

        //    var canSplit = MeasureMent!=null && MeasureMent.MeasureMentType == MeasureMentType.Multiple &&  MeasureMent.MeasureMentUnits.Count() > 1;
        //    data["canSplit"] = canSplit;

        //    return base.FillEntityDataAfter(data, moduleInfo, entity);
        //}
    }

}
