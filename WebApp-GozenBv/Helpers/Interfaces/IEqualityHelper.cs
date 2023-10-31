namespace WebApp_GozenBv.Helpers.Interfaces
{
    public interface IEqualityHelper
    {
        bool AreEqual<T>(T class1, T class2) where T : class;
    }
}