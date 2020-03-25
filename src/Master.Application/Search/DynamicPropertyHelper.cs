using Master.EntityFrameworkCore;
using Master.Module;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Master.Search
{
    public class DynamicPropertyHelper
    {
        public static ColumnTypes GetColumnTypeFromString(string typeName)
        {
            ColumnTypes result;
            switch (typeName.ToLower())
            {
                case "number":
                    result = ColumnTypes.Number;
                    break;
                case "date":
                    result = ColumnTypes.DateTime;
                    break;
                case "bool":
                    result = ColumnTypes.Switch;
                    break;
                default:
                    result = ColumnTypes.Text;
                    break;
            }
            return result;
        }
        /// <summary>
        /// 对属性列生成lamda表达式如o=>MasterDbContext.GetJsonString(o.Property,"$.PY")
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="columnType"></param>
        /// <param name="columnKey"></param>
        /// <returns></returns>
        public static LambdaExpression GeneratePropertyLamda<TEntity>(ColumnTypes columnType, string columnKey)
        {
            return GeneratePropertyLamda<TEntity>(columnType, columnKey, "Property");
        }
        /// <summary>
        /// 对属性列生成lamda表达式
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="columnType"></param>
        /// <param name="columnKey"></param>
        /// <param name="propertyPath">Material.Property</param>
        /// <returns></returns>
        public static LambdaExpression GeneratePropertyLamda<TEntity>(ColumnTypes columnType, string columnKey, string propertyPath)
        {
            var dbFuncName = GetDbFunctionName(columnType);
            var p = Expression.Parameter(typeof(TEntity), "x");
            var _pExp = Expression.Constant($"$.{columnKey}", typeof(string));
            //MemberExpression member = Expression.PropertyOrField(p, "Property");
            MemberExpression member = MakePropertyMemberExpression(p, propertyPath);
            var expRes = Expression.Call(typeof(TEntity).GetEntityDbContextType().GetMethod(dbFuncName), member, _pExp);
            return Expression.Lambda(expRes, p);
        }
        /// <summary>
        /// 递归方式构建属性表达式
        /// </summary>
        /// <param name="p"></param>
        /// <param name="propertyPath">Part.Project.Property</param>
        /// <returns></returns>
        private static MemberExpression MakePropertyMemberExpression(Expression p, string propertyPath)
        {
            if (propertyPath.IndexOf('.') < 0)
            {
                return Expression.PropertyOrField(p, propertyPath);
            }
            else
            {
                //取第一个属性
                var propertyName = propertyPath.Split('.')[0];//Part
                var exp = Expression.PropertyOrField(p, propertyName);
                return MakePropertyMemberExpression(exp, propertyPath.Replace(propertyName + ".", ""));
            }
        }
        /// <summary>
        /// 获取列类型对应的自定义属性数据库函数名称
        /// </summary>
        /// <param name="columnType"></param>
        /// <returns></returns>
        public static string GetDbFunctionName(ColumnTypes columnType)
        {
            string result;
            switch (columnType)
            {
                case ColumnTypes.DateTime:
                    result = nameof(MasterDbContext.GetJsonValueDate);
                    break;
                case ColumnTypes.Number:
                    result = nameof(MasterDbContext.GetJsonValueNumber);
                    break;
                case ColumnTypes.Switch:
                    result = nameof(MasterDbContext.GetJsonValueBool);
                    break;
                default:
                    result = nameof(MasterDbContext.GetJsonValueString);
                    break;
            }
            return result;
        }
    }
}
