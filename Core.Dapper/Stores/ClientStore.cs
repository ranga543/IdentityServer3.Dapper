using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using IdentityServer3.Dapper.Models;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;

namespace IdentityServer3.Dapper
{
    public class BaseClient : Client
    {
        public int Id { get; set; }
    }
    public class ClientStore : BaseStore, IClientStore
    {
        public ClientStore(DapperServiceOptions options) : base(options)
        {
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            using (var con = Options.OpenConnection())
            {
                var baseClient =
                    await con.QueryFirstOrDefaultAsync<BaseClient>("SELECT * FROM Clients WHERE ClientId=@Id",
                        new {Id = clientId});
                var sql =
                @"
                SELECT Origin FROM ClientCorsOrigins WHERE Client_Id = @id
                SELECT GrantType FROM ClientCustomGrantTypes WHERE Client_Id = @id
                SELECT Scope FROM ClientScopes WHERE Client_Id = @id
                SELECT Type, Value FROM ClientClaims WHERE Client_Id = @id
                SELECT Value, Type, Description, Expiration FROM ClientSecrets WHERE Client_Id = @id
                SELECT Provider FROM ClientIdPRestrictions WHERE Client_Id = @id
                SELECT Uri FROM ClientPostLogoutRedirectUris WHERE Client_Id = @id
                SELECT Uri FROM ClientRedirectUris WHERE Client_Id = @id";
                using (var multi = await con.QueryMultipleAsync(sql, new {baseClient.Id}))
                {
                    //var client = multi.Read<Client>().Single();
                    baseClient.AllowedCorsOrigins = multi.Read<string>().ToList();
                    baseClient.AllowedCustomGrantTypes = multi.Read<string>().ToList();
                    baseClient.AllowedScopes = multi.Read<string>().ToList();
                    baseClient.Claims = new List<Claim>();
                    var claims = multi.Read<ClaimReader>();
                    foreach (var claimReader in claims)
                    {
                        baseClient.Claims.Add(new Claim(claimReader.Type, claimReader.Value));
                    }
                    baseClient.ClientSecrets = multi.Read<Secret>().ToList();
                    baseClient.IdentityProviderRestrictions = multi.Read<string>().ToList();
                    baseClient.PostLogoutRedirectUris = multi.Read<string>().ToList();
                    baseClient.RedirectUris = multi.Read<string>().ToList();
                }

                return baseClient;
            }
            
        }
    }
}