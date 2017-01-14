using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IdentityServer3.Dapper.Models;
using IdentityServer3.Dapper.Serialization;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Newtonsoft.Json;
using Token = IdentityServer3.Dapper.Models.Token;

namespace IdentityServer3.Dapper
{
    public abstract class BaseTokenStore<T> where T : class
    {
        protected readonly DapperServiceOptions Options;
        protected readonly TokenType TokenType;
        protected readonly IScopeStore ScopeStore;
        protected readonly IClientStore ClientStore;

        protected BaseTokenStore(DapperServiceOptions options, TokenType tokenType, IScopeStore scopeStore, IClientStore clientStore)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (scopeStore == null) throw new ArgumentNullException(nameof(scopeStore));
            if (clientStore == null) throw new ArgumentNullException(nameof(clientStore));
            Options = options;
            TokenType = tokenType;
            ScopeStore = scopeStore;
            ClientStore = clientStore;
        }

       

        JsonSerializerSettings GetJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ClaimConverter());
            settings.Converters.Add(new ClaimsPrincipalConverter());
            settings.Converters.Add(new ClientConverter(ClientStore));
            settings.Converters.Add(new ScopeConverter(ScopeStore));
            return settings;
        }

        protected string ConvertToJson(T value)
        {
            return JsonConvert.SerializeObject(value, GetJsonSerializerSettings());
        }

        protected T ConvertFromJson(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, GetJsonSerializerSettings());
        }

        public async Task<T> GetAsync(string key)
        {
            Token token;
            using (var con = Options.OpenConnection())
            {
                token = await con.QueryFirstOrDefaultAsync<Token>("SELECT * FROM Tokens WHERE [Key] = @Key AND TokenType = @TokenType", new {Key = key, TokenType});
            }

            if (token == null || token.Expiry < DateTimeOffset.UtcNow)
            {
                return null;
            }

            return ConvertFromJson(token.JsonCode);
        }

        public async Task RemoveAsync(string key)
        {
            using (var con = Options.OpenConnection())
            {
                await con.ExecuteAsync("DELETE FROM Tokens WHERE [Key] = @Key AND TokenType = @TokenType", new { Key = key, TokenType });
            }
        }

        public async Task<IEnumerable<ITokenMetadata>> GetAllAsync(string subject)
        {
            using (var con = Options.OpenConnection())
            {
                var tokens = await con.QueryAsync<Token>("SELECT * FROM Tokens WHERE SubjectId = @SubjectId AND TokenType = @TokenType", new { SubjectId = subject, TokenType });
                var results = tokens.Select(x => ConvertFromJson(x.JsonCode)).ToArray();
                return results.Cast<ITokenMetadata>();
            }
        }

        public async Task RevokeAsync(string subject, string clientId)
        {
            using (var con = Options.OpenConnection())
            {
                var tokens = await con.ExecuteAsync("DELETE FROM Tokens WHERE SubjectId = @SubjectId AND ClientId = @ClientId AND TokenType = @TokenType", new { SubjectId = subject, ClientId = clientId, tokenType = TokenType });
            }
        }

        public abstract Task StoreAsync(string key, T value);
    }
}