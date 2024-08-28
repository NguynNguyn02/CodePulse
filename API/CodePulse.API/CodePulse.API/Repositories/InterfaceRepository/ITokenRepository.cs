using Microsoft.AspNetCore.Identity;

namespace CodePulse.API.Repositories.InterfaceRepository
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user,List<string> roles);

    }
}
