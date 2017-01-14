using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IdentityServer3.Dapper.Models;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;

namespace IdentityServer3.Dapper
{
    public class ConsentStore : BaseStore, IConsentStore
    {
        public ConsentStore(DapperServiceOptions options) : base(options)
        {
        }

        public async Task<IEnumerable<Consent>> LoadAllAsync(string subject)
        {
            using (var con = Options.OpenConnection())
            {
                var consentReaders = await con.QueryAsync<ConsentReader>("SELECT * FROM Consents WHERE Subject = @Subject", new {Subject = subject});
                return consentReaders.Select<ConsentReader, Consent>(x => x);
            }
        }

        public async Task RevokeAsync(string subject, string client)
        {
            using (var con = Options.OpenConnection())
            {
                var result =
                    await con.ExecuteAsync("DELETE FROM Consents WHERE Subject = @Subject AND ClientId = @ClientId",
                        new {Subject = subject, Client = client});
            }
        }

        public async Task<Consent> LoadAsync(string subject, string client)
        {
            using (var con = Options.OpenConnection())
            {
                var consentReader = await con.QueryFirstOrDefaultAsync<ConsentReader>("SELECT * FROM Consents WHERE WHERE Subject = @Subject AND ClientId = @ClientId",
                        new { Subject = subject, Client = client });
                return consentReader;
            }
        }

        public async Task UpdateAsync(Consent consent)
        {
            using (var con = Options.OpenConnection())
            {
                var result =
                    await con.ExecuteAsync("UPDATE Consents SET Scopes = @Scopes  WHERE Subject = @Subject AND ClientId = @ClientId",
                        new { consent.Subject, consent.ClientId, Scopes = ConsentReader.StringifyScopes(consent.Scopes) });
            }
        }
    }
}