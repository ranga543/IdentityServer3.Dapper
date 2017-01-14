using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;

namespace IdentityServer3.Dapper
{
    public class BaseScope : Scope
    {
        public int Id { get; set; }
    }
    public class ScopeStore : BaseStore, IScopeStore
    {
        public ScopeStore(DapperServiceOptions options) : base(options)
        {
        }

        public async Task<IEnumerable<Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
        {
            using (var con = Options.OpenConnection())
            {
                var scopes = await con.QueryAsync<BaseScope>("SELECT * FROM Scopes WHERE Name IN @scopeNames", new {scopeNames});
                var sql = @"
                        SELECT Name, Description, AlwaysIncludeInIdToken FROM ScopeClaims WHERE Scope_Id = @Id
                        SELECT Description, Expiration, Type, Value FROM ScopeSecrets WHERE Scope_Id = @Id
                    ";
                foreach (var baseScope in scopes)
                {
                    using (var multi = await con.QueryMultipleAsync(sql, new {baseScope.Id}))
                    {
                        baseScope.Claims = multi.Read<ScopeClaim>().ToList();
                        baseScope.ScopeSecrets = multi.Read<Secret>().ToList();
                    }
                }
                return scopes;
            }
        }

        public async Task<IEnumerable<Scope>> GetScopesAsync(bool publicOnly = true)
        {
            using (var con = Options.OpenConnection())
            {
                var scopes = await con.QueryAsync<BaseScope>("SELECT * FROM Scopes WHERE ShowInDiscoveryDocument = @publicOnly", new { publicOnly });
                var sql = @"
                        SELECT Name, Description, AlwaysIncludeInIdToken FROM ScopeClaims WHERE Scope_Id = @Id
                        SELECT Description, Expiration, Type, Value FROM ScopeSecrets WHERE Scope_Id = @Id
                    ";
                foreach (var baseScope in scopes)
                {
                    using (var multi = await con.QueryMultipleAsync(sql, new { baseScope.Id }))
                    {
                        baseScope.Claims = multi.Read<ScopeClaim>().ToList();
                        baseScope.ScopeSecrets = multi.Read<Secret>().ToList();
                    }
                }
                return scopes;
            }
        }
    }
}