using System;
using System.Threading.Tasks;
using Dapper;
using IdentityServer3.Dapper.Models;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Token = IdentityServer3.Dapper.Models.Token;

namespace IdentityServer3.Dapper
{
    public class AuthorizationCodeStore : BaseTokenStore<AuthorizationCode>, IAuthorizationCodeStore
    {
        public AuthorizationCodeStore(DapperServiceOptions options, IScopeStore scopeStore, IClientStore clientStore)
            : base(options, TokenType.AuthorizationCode, scopeStore, clientStore)
        {
        }

        public override async Task StoreAsync(string key, AuthorizationCode code)
        {
            var token = new Token
            {
                Key = key,
                SubjectId = code.SubjectId,
                ClientId = code.ClientId,
                JsonCode = ConvertToJson(code),
                Expiry = DateTimeOffset.UtcNow.AddSeconds(code.Client.AuthorizationCodeLifetime),
                TokenType = TokenType
            };
            using (var con = Options.OpenConnection())
            {
                var r = await con.ExecuteAsync("INSERT INTO Tokens ([Key], SubjectId, ClientId, JsonCode, Expiry, TokenType) VALUES (@Key, @SubjectId, @ClientId, @JsonCode, @Expiry, @TokenType)", token);
            }
        }
    }
}