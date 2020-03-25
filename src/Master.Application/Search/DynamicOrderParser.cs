using Master.Entity;
using Master.EntityFrameworkCore;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Master.Search
{
    public class DynamicOrderParser : IDynamicOrderParser
    {
        public IQueryable Parse<TEntity>(string orderField, SortType sortType, ModuleInfo moduleInfo, IQueryable query)
        {
            if (moduleInfo == null)
            {
                //没有模块信息，则直接解析字符串:property.ExpireDate
                if (orderField.ToLower().StartsWith("property"))
                {
                    query= GeneratePropertyOrderQuery<TEntity>(new Regex("property\\.", RegexOptions.IgnoreCase).Replace(orderField, ""), sortType,query);
                }
                else
                {
                    query = query.OrderBy($"{orderField} {sortType}");
                }
            }
            else
            {
                //是模块排序，则去列定义中寻找具体的排序字段
                var column = moduleInfo.ColumnInfos.SingleOrDefault(o => o.ColumnKey.ToLower() == orderField.ToLower());
                if (column == null)
                {
                    query = query.OrderBy($"{orderField} {sortType}");
                }
                else if (column.IsPropertyColumn)
                {
                    query = GeneratePropertyOrderQuery<TEntity>(column.ColumnKey, sortType, query);                    
                }
                else
                {
                    query = query.OrderBy($"{GetColumnOrderField(column)} {sortType}");
                }
            }
            return query;
        }

        private IQueryable GeneratePropertyOrderQuery<TEntity>(string columnKey,SortType sortType,IQueryable query)
        {
            var expression = (Expression<Func<TEntity, string>>)DynamicPropertyHelper.GeneratePropertyLamda<TEntity>(ColumnTypes.Text, columnKey);
            if (sortType == SortType.Asc)
            {
                //query = (query as IQueryable<TEntity>).OrderBy(o => MasterDbContext.GetJsonValueString(o.Property, $"$.{columnKey}"));
                query = (query as IQueryable<TEntity>).OrderBy(expression);
            }
            else
            {
                //query = (query as IQueryable<TEntity>).OrderByDescending(o => MasterDbContext.GetJsonValueString(o.Property, $"$.{columnKey}"));
                query = (query as IQueryable<TEntity>).OrderByDescending(expression);
            }
            //(query as IQueryable<TEntity>).OrderBy(DynamicPropertyHelper.GeneratePropertyLamda<TEntity>(ColumnTypes.Text,columnKey))
            return query;
        }

        /// <summary>
        /// 获取列的排序字段
        /// </summary>
        /// <param name="columnInfo"></param>
        /// <returns></returns>
        private string GetColumnOrderField(ColumnInfo columnInfo)
        {
            return string.IsNullOrEmpty(columnInfo.DisplayPath) ? columnInfo.ValuePath : columnInfo.DisplayPath;
        }
    }
}
