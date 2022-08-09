using Ideal.Core.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Ideal.Core.Common.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class TableHelper
    {
        /// <summary>
        /// 根据类创建表格
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <returns>DataTable</returns>
        public static DataTable Create<T>()
        {
            return Create<T>(null);
        }

        /// <summary>
        /// 根据类创建表格
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="tableName">表名</param>
        /// <returns>DataTable</returns>
        public static DataTable Create<T>(string tableName)
        {
            var entity = Activator.CreateInstance<T>();
            var propertys = entity.GetType().GetProperties().Where(u => u.CanWrite);
            var cols = new Dictionary<string, Type>();
            foreach (var p in propertys)
            {
                cols.Add(p.Name, p.PropertyType);
            }

            return Create(tableName, cols);
        }

        /// <summary>
        /// 通过类属性特性创建表格（将设置了ExportColumnAttribute特性的属性的特性值作为表格的列名称）
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <returns>DataTable</returns>
        public static DataTable CreateWithPropertyDescription<T>()
        {
            return CreateWithPropertyDescription<T>(null);
        }

        /// <summary>
        /// 将设置了ExportColumnAttribute特性的属性的特性值作为表格的列名称
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="tableName">表名</param>
        /// <returns>DataTable</returns>
        public static DataTable CreateWithPropertyDescription<T>(string tableName)
        {
            var entity = Activator.CreateInstance<T>();
            var propertys = entity.GetType().GetProperties().Where(u => u.CanWrite);
            var cols = new Dictionary<string, Type>();
            foreach (var p in propertys)
            {
                var descriptionAttr = p.GetCustomAttributes(typeof(ColumnNameAttribute), true).FirstOrDefault();
                if (descriptionAttr != null)
                {
                    try
                    {
                        var pdescription = ((ColumnNameAttribute)descriptionAttr).TableColumnName;
                        cols.Add(pdescription, p.PropertyType);
                    }
                    catch { }
                }
                else
                {
                    {
                        continue;
                        //throw new InvalidOperationException($"请为属性{p.Name}设置ExportColumnAttribute");
                    }
                }
            }

            return Create(tableName, cols);
        }

        /// <summary>
        /// 根据列名数组创建表格
        /// </summary>
        /// <param name="cols">列名数组</param>
        /// <returns>DataTable</returns>
        public static DataTable Create(string[] cols)
        {
            return Create(null, cols);
        }

        /// <summary>
        /// 根据列名数组创建表格
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">列名数组</param>
        /// <returns>DataTable</returns>
        public static DataTable Create(string tableName, string[] cols)
        {
            var tb = new DataTable(tableName);
            foreach (var col in cols)
            {
                tb.Columns.Add(col, Type.GetType("System.String", false, false));
            }

            return tb;
        }

        /// <summary>
        /// 根据列名集合（包含列名及数据类型）创建表格
        /// </summary>
        /// <param name="cols">列名集合（包含列名及数据类型）</param>
        /// <returns>DataTable</returns>
        public static DataTable Create(Dictionary<string, Type> cols)
        {
            return Create(null, cols);
        }

        /// <summary>
        /// 根据列名集合（包含列名及数据类型）创建表格
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="cols">列名集合（包含列名及数据类型）</param>
        /// <returns>DataTable</returns>
        public static DataTable Create(string tableName, Dictionary<string, Type> cols)
        {
            var tb = new DataTable(tableName);
            foreach (var col in cols)
            {
                tb.Columns.Add(col.Key, col.Value);
            }

            return tb;
        }

        /// <summary>
        /// 把实体对象转为表格
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="model">实体对象</param>
        /// <returns>DataTable</returns>
        public static DataTable FromEntity<T>(T model)
        {
            return FromEntity(null, model);
        }

        /// <summary>
        /// 把实体对象转为表格
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="model">实体对象</param>
        /// <returns>DataTable</returns>
        public static DataTable FromEntity<T>(string tableName, T model)
        {
            var dt = Create<T>(tableName);
            var propertys = Activator.CreateInstance<T>().GetType().GetProperties().Where(u => u.CanWrite);
            FillDataRowByEntity(dt, model, propertys);
            return dt;
        }


        /// <summary>
        /// 把实体对象转为表格
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="model">实体对象</param>
        /// <returns>DataTable</returns>
        public static DataTable FromEntityWithPropertyDescription<T>(T model)
        {
            return FromEntityWithPropertyDescription(null, model);
        }

        /// <summary>
        /// 把实体对象转为表格
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="model">实体对象</param>
        /// <returns>DataTable</returns>
        public static DataTable FromEntityWithPropertyDescription<T>(string tableName, T model)
        {
            var dt = CreateWithPropertyDescription<T>(tableName);
            var propertys = Activator.CreateInstance<T>().GetType().GetProperties().Where(u => u.CanWrite);
            FillDataRowByEntityWithPropertyDescription(dt, model, propertys);
            return dt;
        }

        /// <summary>
        /// 把一个一维数组转换为DataTable
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="array">一维数组</param>
        /// <returns>返回DataTable</returns>
        public static DataTable FromArray(string columnName, string[] array)
        {
            var dt = new DataTable();
            dt.Columns.Add(columnName, typeof(string));

            foreach (var t in array)
            {
                var dr = dt.NewRow();
                dr[columnName] = t;
                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        /// 把实体对象集合转为表格
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="models">实体对象集合</param>
        /// <returns>DataTable</returns>
        public static DataTable FromEntity<T>(IEnumerable<T> models)
        {
            return FromEntity(null, models);
        }

        /// <summary>
        /// 把实体对象集合转为表格
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="models">实体对象集合</param>
        /// <returns>DataTable</returns>
        public static DataTable FromEntity<T>(string tableName, IEnumerable<T> models)
        {
            if (models == null || !models.Any())
            {
                return null;
            }

            var propertys = Activator.CreateInstance<T>().GetType().GetProperties().Where(u => u.CanWrite);
            var dt = Create<T>(tableName);
            foreach (var model in models)
            {
                FillDataRowByEntity(dt, model, propertys);
            }

            return dt;
        }

        /// <summary>
        /// 把实体对象集合转为表格
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="models">实体对象集合</param>
        /// <returns>DataTable</returns>
        public static DataTable FromEntityWithPropertyDescription<T>(IEnumerable<T> models)
        {
            return FromEntityWithPropertyDescription(null, models);
        }

        /// <summary>
        /// 把实体对象集合转为表格
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="models">实体对象集合</param>
        /// <returns>DataTable</returns>
        public static DataTable FromEntityWithPropertyDescription<T>(string tableName, IEnumerable<T> models)
        {
            if (models == null || !models.Any())
            {
                return null;
            }

            var propertys = Activator.CreateInstance<T>().GetType().GetProperties().Where(u => u.CanWrite);
            var dt = CreateWithPropertyDescription<T>(tableName);
            foreach (var model in models)
            {
                FillDataRowByEntityWithPropertyDescription(dt, model, propertys);
            }
            return dt;
        }

        private static void FillDataRowByEntity<T>(DataTable dt, T model, IEnumerable<PropertyInfo> propertys)
        {
            var dr = dt.NewRow();
            foreach (var p in propertys)
            {
                dr[p.Name] = p.GetValue(model, null);
            }

            dt.Rows.Add(dr);
        }

        private static void FillDataRowByEntityWithPropertyDescription<T>(DataTable dt, T model, IEnumerable<PropertyInfo> propertys)
        {
            var dr = dt.NewRow();
            foreach (var p in propertys)
            {
                var descriptionAttr = p.GetCustomAttributes(typeof(ColumnNameAttribute), true).FirstOrDefault();
                if (descriptionAttr != null)
                {
                    try
                    {
                        var pdescription = ((ColumnNameAttribute)descriptionAttr).TableColumnName;
                        dr[pdescription] = p.GetValue(model, null);
                    }
                    catch { }
                }
                else
                {
                    continue;
                    //throw new InvalidOperationException($"请为属性{p.Name}设置ExportColumnAttribute");
                }
            }

            dt.Rows.Add(dr);
        }

        /// <summary>
        /// 把表格转换为实体对象集合
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="dt">表</param>
        /// <returns>实体对象集合</returns>
        public static IEnumerable<T> ToEntities<T>(DataTable dt)
        {
            var list = new List<T>();
            if (0 == dt.Rows.Count)
            {
                return list;
            }

            var entity = Activator.CreateInstance<T>();
            var propertys = entity.GetType().GetProperties().Where(u => u.CanWrite);
            foreach (DataRow dr in dt.Rows)
            {
                entity = ToEntityFromDataRow<T>(dr, propertys);
                list.Add(entity);
            }

            return list;
        }

        /// <summary>
        /// 把表格转换为实体对象集合
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="dt">表</param>
        /// <returns>实体对象集合</returns>
        public static IEnumerable<T> ToEntitiesWithPropertyDescription<T>(DataTable dt)
        {
            var list = new List<T>();
            if (0 == dt.Rows.Count)
            {
                return list;
            }

            var entity = Activator.CreateInstance<T>();
            var propertys = entity.GetType().GetProperties().Where(u => u.CanWrite);
            foreach (DataRow dr in dt.Rows)
            {
                entity = ToEntityFromDataRowWithPropertyDescription<T>(dr, propertys);
                list.Add(entity);
            }

            return list;
        }

        /// <summary>
        /// 把表格行记录转为实体对象
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="dr">表格行记录</param>
        /// <returns>实体对象</returns>
        public static T ToEntityFromDataRow<T>(DataRow dr)
        {
            return ToEntityFromDataRow<T>(dr, null);
        }

        /// <summary>
        /// 把表格行记录转为实体对象
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="dr">表格行记录</param>
        /// <returns>实体对象</returns>
        public static T ToEntityFromDataRowWithPropertyDescription<T>(DataRow dr)
        {
            return ToEntityFromDataRowWithPropertyDescription<T>(dr, null);
        }

        private static T ToEntityFromDataRow<T>(DataRow dr, IEnumerable<PropertyInfo> propertys)
        {
            var entity = Activator.CreateInstance<T>();
            propertys ??= entity.GetType().GetProperties().Where(u => u.CanWrite);
            foreach (var p in propertys)
            {
                var key = p.Name;
                if (!dr.Table.Columns.Contains(key))
                {
                    continue;
                }

                var value = dr[key];
                if (value != DBNull.Value)
                {
                    p.SetValue(entity, value, null);
                }
            }

            return entity;
        }

        private static T ToEntityFromDataRowWithPropertyDescription<T>(DataRow dr, IEnumerable<PropertyInfo> propertys)
        {
            var entity = Activator.CreateInstance<T>();
            propertys ??= entity.GetType().GetProperties().Where(u => u.CanWrite);
            foreach (var p in propertys)
            {
                var descriptionAttr = p.GetCustomAttributes(typeof(ColumnNameAttribute), true).FirstOrDefault();
                if (descriptionAttr != null)
                {
                    try
                    {
                        var key = ((ColumnNameAttribute)descriptionAttr).TableColumnName;
                        if (!dr.Table.Columns.Contains(key))
                        {
                            continue;
                        }

                        var value = dr[key];
                        if (value != DBNull.Value)
                        {
                            p.SetValue(entity, value, null);
                        }
                    }
                    catch { }
                }
                else
                {
                    continue;
                    //throw new InvalidOperationException($"请为属性{p.Name}设置ExportColumnAttribute");
                }

            }

            return entity;
        }

        public static DataTable RowColTranspose(DataTable dt, string columnName)
        {
            return RowColTranspose(null, dt, columnName);
        }

        public static DataTable RowColTranspose(string tableName, DataTable dt, string columnName)
        {
            return !dt.Columns.Contains(columnName) ? null : RowColTranspose(tableName, dt, dt.Columns.IndexOf(columnName));
        }

        public static DataTable RowColTranspose(DataTable dt, int columnIndex)
        {
            return RowColTranspose(null, dt, columnIndex);
        }

        public static DataTable RowColTranspose(string tableName, DataTable dt, int columnIndex)
        {
            if (dt.Columns.Count - 1 < columnIndex)
            {
                return null;
            }

            var dtNew = new DataTable();
            var col = dt.Columns[columnIndex];
            dtNew.Columns.Add(col.ColumnName);

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                dtNew.Columns.Add(dt.Rows[i][col].ToString());
            }

            foreach (DataColumn column in dt.Columns)
            {
                var newRow = new object[dt.Rows.Count + 1];
                newRow[0] = column.ColumnName;
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    newRow[i + 1] = dt.Rows[i][column];
                }

                dtNew.Rows.Add(newRow);
            }

            return dtNew;
        }
    }
}
