using System;
using IdentityServer3.Core.Services;
using IdentityServer3.Dapper;

namespace IdentityServer3.Core.Configuration
{
    public static class IdentityServerServiceFactoryExtensions
    {
        public static void RegisterOperationalServices(this IdentityServerServiceFactory factory, DapperServiceOptions options)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (options == null) throw new ArgumentNullException(nameof(options));

            factory.Register(new Registration<DapperServiceOptions>(options));

            factory.AuthorizationCodeStore = new Registration<IAuthorizationCodeStore, AuthorizationCodeStore>();
            factory.TokenHandleStore = new Registration<ITokenHandleStore, TokenHandleStore>();
            factory.ConsentStore = new Registration<IConsentStore, ConsentStore>();
            factory.RefreshTokenStore = new Registration<IRefreshTokenStore, RefreshTokenStore>();
        }

        public static void RegisterConfigurationServices(this IdentityServerServiceFactory factory, DapperServiceOptions options)
        {
            factory.RegisterClientStore(options);
            factory.RegisterScopeStore(options);
        }

        public static void RegisterClientStore(this IdentityServerServiceFactory factory, DapperServiceOptions options)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (options == null) throw new ArgumentNullException(nameof(options));

            factory.Register(new Registration<DapperServiceOptions>(options));

            factory.ClientStore = new Registration<IClientStore, ClientStore>();
            factory.CorsPolicyService = new Registration<ICorsPolicyService, ClientConfigurationCorsPolicyService>();
        }

        public static void RegisterScopeStore(this IdentityServerServiceFactory factory, DapperServiceOptions options)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (options == null) throw new ArgumentNullException(nameof(options));

            factory.Register(new Registration<DapperServiceOptions>(options));

            factory.ScopeStore = new Registration<IScopeStore, ScopeStore>();
        }
    }
}
