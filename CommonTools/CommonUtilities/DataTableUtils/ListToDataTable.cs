using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.DataTableUtils
{
    public static class ListToDataTable
    {
     /// <summary>
     /// Extension method to convert list to datatable
     /// </summary>
     /// <typeparam name="T"></typeparam>
     /// <param name="list"></param>
     /// <returns></returns>
        public static DataTable ConvertToDataTable<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0) return new DataTable();

            if (list.First() is IDynamicMetaObjectProvider) //If list contains an ExpandoObject 
            {
                return ConvertToDataTableFromExpando(list);
            }
            else
            {
                DataTable table = CreateTable<T>();
                Type entityType = typeof(T);
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

                foreach (T item in list)
                {
                    DataRow row = table.NewRow();

                    foreach (PropertyDescriptor prop in properties)
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }

                    table.Rows.Add(row);
                }
                return table;
            }
        }



        private static DataTable ConvertToDataTableFromExpando<T>(this IList<T> list)
        {
            var expando = list.First() as IDictionary<string, object>;
            ICollection<string> propertyNames = expando.Keys;            
            //Get list of corresponding Types
            List<Type> properyTypes = expando.Values.ToList().Select(obj => (obj != null) ? obj.GetType() : typeof(Object)).ToList();
            DataTable table = CreateTable(propertyNames, properyTypes);
                
            foreach (T item in list)
            {
                var dynamicItem = item as IDictionary<string, object>;
                var propertyValues = dynamicItem.Values.ToList();

                DataRow row = table.NewRow();
                int index = 0; 
                foreach (string propertyName in propertyNames)
                {
                    row[propertyName] = propertyValues[index] ?? DBNull.Value;
                    index++;
                }
                table.Rows.Add(row);
            }
            return table;
        }
                

        public static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            return table;
        }

        private static DataTable CreateTable(ICollection<string> propertyNames, List<Type> properyTypes)
        {
            DataTable table = new DataTable();
            int index = 0; 
            foreach (var prop in propertyNames)
            {
                table.Columns.Add(prop, properyTypes[index++]);
            }
            return table;
        }


        
    }
}
