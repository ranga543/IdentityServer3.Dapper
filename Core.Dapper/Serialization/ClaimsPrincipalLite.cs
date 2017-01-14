using IdentityServer3.Dapper.Models;

namespace IdentityServer3.Dapper.Serialization
{
    public class ClaimsPrincipalLite
    {
        public string AuthenticationType { get; set; }
        public ClaimReader[] Claims { get; set; }
    }
}