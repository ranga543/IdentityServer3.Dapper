using System.Collections.Generic;
using System.Linq;
using IdentityServer3.Core.Models;

namespace IdentityServer3.Dapper.Models
{
    public class ConsentReader
    {
        public string ClientId { get; set; }
        public string Scopes { get; set; }
        public string Subject { get; set; }

        public static implicit operator Consent(ConsentReader reader)
        {
            return new Consent
            {
                ClientId = reader.ClientId,
                Subject = reader.Subject,
                Scopes = ParseScopes(reader.Scopes)
            };
        }

        public static IEnumerable<string> ParseScopes(string scopes)
        {
            if (string.IsNullOrWhiteSpace(scopes))
            {
                return Enumerable.Empty<string>();
            }

            return scopes.Split(',');
        }

        public static string StringifyScopes(IEnumerable<string> scopes)
        {
            if (scopes == null || !scopes.Any())
            {
                return null;
            }

            return scopes.Aggregate((s1, s2) => s1 + "," + s2);
        }
    }
}