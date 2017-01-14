using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Dapper.Sample.Auth;
using IdentityServer3.Dapper.Sample.Auth.Repositories;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace IdentityServer3.Dapper.Sample.Auth
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var dapperServiceOptions = new DapperServiceOptions(() => new SqlConnection(ConfigurationManager.ConnectionStrings["Sample.Idsvr"].ConnectionString));

            var userRepository = new UserRepository(
                () => new SqlConnection(ConfigurationManager.ConnectionStrings["Sample"].ConnectionString)
            );

            var factory = new IdentityServerServiceFactory();
            factory.RegisterConfigurationServices(dapperServiceOptions);
            factory.RegisterOperationalServices(dapperServiceOptions);
            factory.UserService = new Registration<IUserService>(
                typeof(SampleUserService));
            factory.Register(new Registration<IUserRepository>(userRepository));


            var certificate = Convert.FromBase64String(ConfigurationManager.AppSettings.Get("SigningCertificate"));
            var password = ConfigurationManager.AppSettings.Get("SigningCertificatePassword");
            var options = new IdentityServerOptions
            {
                SiteName = "Dapper Identity Server Auth",
                SigningCertificate = new X509Certificate2(certificate, password),
                Factory = factory,
                RequireSsl = false // DO NOT DO THIS
            };
            app.UseIdentityServer(options);
        }
    }
}
