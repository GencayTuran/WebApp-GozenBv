using System.Reflection;
using System;
using WebApp_GozenBv.Helpers.Interfaces;

namespace WebApp_GozenBv.Helpers
{
    public class EqualityHelper : IEqualityHelper
    {
        public bool AreEqual<T>(T class1, T class2) where T : class
        {
            if (class1 == null || class2 == null)
            {
                return false;
            }

            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object value1 = property.GetValue(class1);
                object value2 = property.GetValue(class2);

                if (!object.Equals(value1, value2))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
