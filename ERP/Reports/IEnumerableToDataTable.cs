using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Reports
{
    public static class IEnumerableToDataTable
    {
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);
            try
            {              

                PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var prop in props)
                {
                    tb.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }

                foreach (var item in items)
                {
                    var values = new object[props.Length];
                    for (var i = 0; i < props.Length; i++)
                    {
                        values[i] = props[i].GetValue(item, null);
                    }

                    tb.Rows.Add(values);
                }

            }
            catch 
            {
                
                
            }
            return tb;
        }
    }
}
