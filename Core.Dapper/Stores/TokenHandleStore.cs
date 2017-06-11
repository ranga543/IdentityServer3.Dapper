using System;
using System.Threading.Tasks;
using Dapper;
using IdentityServer3.Dapper.Models;
using IdentityServer3.Core.Services;

namespace IdentityServer3.Dapper
{
    public class TokenHandleStore : BaseTokenStore<IdentityServer3.Core.Models.Token>, ITokenHandleStore
    {
        public TokenHandleStore(DapperServiceOptions options, IScopeStore scopeStore, IClientStore clientStore)
            : base(options, TokenType.RefreshToken, scopeStore, clientStore)
        {
        }

        public override async Task StoreAsync(string key, IdentityServer3.Core.Models.Token value)
        {
            var token = new Token
            {
                Key = key,
                SubjectId = value.SubjectId,
                ClientId = value.ClientId,
                JsonCode = ConvertToJson(value),
                Expiry = DateTimeOffset.UtcNow.AddSeconds(value.Lifetime),
                TokenType = TokenType
            };
            using (var con = Options.OpenConnection())
            {
                var r = await con.ExecuteAsync("INSERT INTO Tokens ([Key], SubjectId, ClientId, JsonCode, Expiry, TokenType) VALUES (@Key, @SubjectId, @ClientId, @JsonCode, @Expiry, @TokenType)", token);
            }
        }
    }
}