namespace eStoreAPI.JWT
{
    public interface IJwtAuth
    {
        string AuthenticateToken(string email, string role, string id = null);
    }
}
