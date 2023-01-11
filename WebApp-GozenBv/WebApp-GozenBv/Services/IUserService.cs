namespace WebApp_GozenBv.Services
{
    public interface IUserService
    {
        int GetCurrentUserId(string userMail, string userName);
    }
}
