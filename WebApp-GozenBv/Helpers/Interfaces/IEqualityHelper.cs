using System.Collections.Generic;
using WebApp_GozenBv.Models;

namespace WebApp_GozenBv.Helpers.Interfaces
{
    public interface IEqualityHelper
    {
        bool AreEqual<T>(T class1, T class2) where T : class;
        bool AreCollectionsEqual<T>(IEnumerable<T> collection1, IEnumerable<T> collection2) where T : class;
        bool AreEditableFieldsEqual(List<MaterialLogItem> original, List<MaterialLogItem> incoming, int status);
    }
}