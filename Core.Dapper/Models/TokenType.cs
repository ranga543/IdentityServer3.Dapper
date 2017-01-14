namespace IdentityServer3.Dapper.Models
{
    public enum TokenType : short
    {
        AuthorizationCode = 1,
        TokenHandle = 2,
        RefreshToken = 3
    }
}