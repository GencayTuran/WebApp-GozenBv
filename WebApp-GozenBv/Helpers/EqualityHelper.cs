using System.Reflection;
using System;
using WebApp_GozenBv.Helpers.Interfaces;
using System.Collections.Generic;
using WebApp_GozenBv.Models;

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

        public bool AreCollectionsEqual<T>(IEnumerable<T> collection1, IEnumerable<T> collection2) where T : class
        {
            if (collection1 == null || collection2 == null)
            {
                return false;
            }

            IEnumerator<T> enumerator1 = collection1.GetEnumerator();
            IEnumerator<T> enumerator2 = collection2.GetEnumerator();

            while (enumerator1.MoveNext() && enumerator2.MoveNext())
            {
                if (!AreEqual(enumerator1.Current, enumerator2.Current))
                {
                    return false;
                }
            }

            // If both collections have the same number of elements and all elements are equal, they are considered equal.
            return !enumerator1.MoveNext() && !enumerator2.MoveNext();
        }

    }
}
