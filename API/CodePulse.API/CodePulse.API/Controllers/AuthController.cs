using CodePulse.API.Models.DTO.Login;
using CodePulse.API.Models.DTO.Register;
using CodePulse.API.Repositories.InterfaceRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }
        
        //POST:{apibaseurl}/api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var identityUser =  await userManager.FindByEmailAsync(request.Email);
            if(identityUser is not null)
            {
                //Check password
                var checkPassword = await userManager.CheckPasswordAsync(identityUser,request.Password);
                if (checkPassword)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);
                    //Create a Token and Response

                    var jwtToken = tokenRepository.CreateJWTToken(identityUser,roles.ToList());


                    var response = new LoginResponseDTO()
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken
                    };

                    return Ok(response);
                }

            }
            ModelState.AddModelError("", "Email or Password incorrect!");
            return ValidationProblem(ModelState);
        }





        // POST: {apibaseurl}/api/auth/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            //Create IdentityUser Object

            var user = new IdentityUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim()
            };


            //Create user

            var identityResult = await userManager.CreateAsync(user, request.Password);
            if (identityResult.Succeeded)
            {

                //Add Role to user (reader)
                identityResult = await userManager.AddToRoleAsync(user, "Reader");
                if (identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                }
            }
            return ValidationProblem(ModelState);
        }
    }
}
