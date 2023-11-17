using System.Reflection;
using System;
using WebApp_GozenBv.Helpers.Interfaces;
using System.Collections.Generic;
using WebApp_GozenBv.Models;
using WebApp_GozenBv.Constants;

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

        public bool AreEditableFieldsEqual(List<MaterialLogItem> original, List<MaterialLogItem> incoming, int status)
        {
            if (original == null || incoming == null)
            {
                throw new ArgumentNullException("One or more collections are null.");
            }

            if (original.Count != incoming.Count)
            {
                return false;
            }

            switch (status)
            {
                case MaterialLogStatus.Created:
                    for (int i = 0; i < original.Count; i++)
                    {
                        if (original[i].MaterialId != incoming[i]?.MaterialId
                            || original[i].MaterialAmount != incoming[i]?.MaterialAmount
                            || original[i].Used != incoming[i]?.Used)
                        {
                            return false;
                        }
                    }
                    break;
                case MaterialLogStatus.Returned:
                    for (int i = 0; i < original.Count; i++)
                    {
                        if (original[i].MaterialId != incoming[i]?.MaterialId
                            || original[i].MaterialAmount != incoming[i]?.MaterialAmount
                            || original[i].Used != incoming[i]?.Used
                            || original[i].IsDamaged != incoming[i]?.IsDamaged
                            || original[i].DamagedAmount != incoming[i]?.DamagedAmount
                            || original[i].RepairAmount != incoming[i]?.RepairAmount
                            || original[i].DeleteAmount != incoming[i]?.DeleteAmount)
                        {
                            return false;
                        }
                    }
                    break;
                default:
                    throw new Exception($"Status id {status} is invalid.");
            }
            return true;
        }
    }
}
