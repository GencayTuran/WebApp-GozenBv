using System.Collections.Generic;

namespace WebApp_GozenBv.Helpers.Interfaces
{
    public interface IEqualityHelper
    {
        bool AreEqual<T>(T class1, T class2) where T : class;
        bool AreCollectionsEqual<T>(IEnumerable<T> collection1, IEnumerable<T> collection2) where T : class;
    }
}