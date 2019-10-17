using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Classes
{
    public static class Utils
    {
        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(anonymousObject))
            {
                var obj = propertyDescriptor.GetValue(anonymousObject);
                expando.Add(propertyDescriptor.Name, obj);
            }

            return (ExpandoObject)expando;
        }
    }
}