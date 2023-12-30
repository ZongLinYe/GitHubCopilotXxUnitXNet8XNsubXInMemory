using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseAPITest
{
    public static class ObjectExtension
    {
        public static ExpandoObject ToExpandoObject(this object obj)
        {
            var expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj.GetType()))
            {
                expando.TryAdd(property.Name, property.GetValue(obj));
            }

            return (ExpandoObject)expando;
        }

    }
}