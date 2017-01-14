using System;
using System.Linq;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Newtonsoft.Json;

namespace IdentityServer3.Dapper.Serialization
{
    public class ScopeConverter : JsonConverter
    {
        private readonly IScopeStore _scopeStore;

        public ScopeConverter(IScopeStore scopeStore)
        {
            if (scopeStore == null) throw new ArgumentNullException(nameof(scopeStore));

            _scopeStore = scopeStore;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Scope) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<ScopeLite>(reader);
            var scopes = AsyncHelper.RunSync(async () => await _scopeStore.FindScopesAsync(new[] { source.Name }));
            return scopes.Single();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (Scope)value;

            var target = new ScopeLite
            {
                Name = source.Name
            };
            serializer.Serialize(writer, target);
        }
    }
}