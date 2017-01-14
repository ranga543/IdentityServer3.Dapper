using System.Threading.Tasks;
using Dapper;
using IdentityServer3.Dapper.Models;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Token = IdentityServer3.Dapper.Models.Token;

namespace IdentityServer3.Dapper
{
    public class RefreshTokenStore : BaseTokenStore<RefreshToken>, IRefreshTokenStore
    {
        public RefreshTokenStore(DapperServiceOptions options, IScopeStore scopeStore, IClientStore clientStore)
            : base(options, TokenType.RefreshToken, scopeStore, clientStore)
        {
        }

        public override async Task StoreAsync(string key, RefreshToken value)
        {
            using (var con = Options.OpenConnection())
            {
                var token = await con.QueryFirstOrDefaultAsync<Token>("SELECT * FROM Tokens WHERE [Key] = @Key AND TokenType = @TokenType", new { Key = key, TokenType });

                if (token == null)
                {
                    token = new Token
                    {
                        Key = key,
                        SubjectId = value.SubjectId,
                        ClientId = value.ClientId,
                        TokenType = TokenType,
                        JsonCode = ConvertToJson(value),
                        Expiry = value.CreationTime.AddSeconds(value.LifeTime)
                    };
                    await
                        con.ExecuteAsync(
                            "INSERT INTO Tokens ([Key], SubjectId, ClientId, JsonCode, Expiry, TokenType) VALUES (@Key, @SubjectId, @ClientId, @JsonCode, @Expiry, @TokenType)",
                            token);
                }
                else
                {

                    token.JsonCode = ConvertToJson(value);
                    token.Expiry = value.CreationTime.AddSeconds(value.LifeTime);
                    await
                        con.ExecuteAsync(
                            "UPDATE Tokens SET JsonCode=@JsonCode, Expiry=@Expiry WHERE [Key] = @Key, TokenType = @TokenType WHERE [Key]=@Key AND TokenType = @TokenType",
                            token);
                }

            }

        }
    }
}