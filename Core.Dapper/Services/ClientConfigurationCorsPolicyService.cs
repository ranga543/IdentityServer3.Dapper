using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IdentityServer3.Core.Services;

namespace IdentityServer3.Dapper
{
    public class ClientConfigurationCorsPolicyService : BaseStore, ICorsPolicyService
    {

        public ClientConfigurationCorsPolicyService(DapperServiceOptions options):base(options)
        {
        }

        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            using (var con = Options.OpenConnection())
            {
                var urls = await con.QueryAsync<string>("SELECT Origin FROM ClientCorsOrigins");

                var origins = urls.ToArray().Select(x => x.GetOrigin()).Where(x => x != null).Distinct();

                var result = origins.Contains(origin, StringComparer.OrdinalIgnoreCase);

                return result;
            }
            
        }
    }
}