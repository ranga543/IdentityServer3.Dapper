using System;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Newtonsoft.Json;

namespace IdentityServer3.Dapper.Serialization
{
    public class ClientConverter : JsonConverter
    {
        private readonly IClientStore _clientStore;

        public ClientConverter(IClientStore clientStore)
        {
            if (clientStore == null) throw new ArgumentNullException(nameof(clientStore));

            _clientStore = clientStore;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Client) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var source = serializer.Deserialize<ClientLite>(reader);
            return AsyncHelper.RunSync(async () => await _clientStore.FindClientByIdAsync(source.ClientId));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = (Client)value;

            var target = new ClientLite
            {
                ClientId = source.ClientId
            };
            serializer.Serialize(writer, target);
        }
    }
}