using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.Dapper.Sample.Auth.Repositories;

namespace IdentityServer3.Dapper.Sample.Auth
{
    public class SampleUserService : UserServiceBase
    {
        private readonly IUserRepository _userRepository;

        public SampleUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public override async Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            var user = await _userRepository.GetAsync(context.UserName,
                HashHelper.Sha512(context.Password));

            context.AuthenticateResult = user == null ? new AuthenticateResult("Incorrect credentials") : new AuthenticateResult(user.Username, user.Username);
        }
    }
}