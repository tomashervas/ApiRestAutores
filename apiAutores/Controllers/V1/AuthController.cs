using apiAutores.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace apiAutores.Controllers.V1
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ResponseAuth>> Register(UserCredentials userCredentials)
        {
            var user = new IdentityUser { UserName = userCredentials.Email, Email = userCredentials.Email };
            var res = await userManager.CreateAsync(user, userCredentials.Password);

            if (res.Succeeded)
            {
                return await GenerateToken(userCredentials);
            }
            else
            {
                return BadRequest(res.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseAuth>> Login(UserCredentials userCredentials)
        {
            var res = await signInManager.PasswordSignInAsync(userCredentials.Email, userCredentials.Password, isPersistent: false, lockoutOnFailure: false);
            if (res.Succeeded)
            {
                return await GenerateToken(userCredentials);
            }
            else
            {
                return BadRequest("Login error");
            }


        }

        [HttpGet("RefreshToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ResponseAuth>> RefreshToken()
        {
            var emailClaim = HttpContext.User.Claims.Where(x => x.Type == "email").FirstOrDefault();
            var email = emailClaim!.Value;
            return await GenerateToken(new UserCredentials() { Email = email });
        }

        private async Task<ResponseAuth> GenerateToken(UserCredentials userCredentials)
        {
            var claims = new List<Claim>()
            {
                new("email", userCredentials.Email)
            };

            var user = await userManager.FindByEmailAsync(userCredentials.Email);
            var claimsDB = await userManager.GetClaimsAsync(user!);
            claims.AddRange(claimsDB);


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SECRET_JWT"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddYears(1);
            var token = new JwtSecurityToken(
                            issuer: null,
                            audience: null,
                            claims: claims,
                            expires: expiration,
                            signingCredentials: creds
                        );

            return new ResponseAuth()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        [HttpPost("MakeAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task<ActionResult> MakeAdmin(EditAdmin editAdmin)
        {
            var user = await userManager.FindByEmailAsync(editAdmin.Email);
            await userManager.AddClaimAsync(user!, new Claim("IsAdmin", "1"));
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        [HttpPost("RemoveAdmin")]
        public async Task<ActionResult> RemoveAdmin(EditAdmin editAdmin)
        {
            var user = await userManager.FindByEmailAsync(editAdmin.Email);
            await userManager.RemoveClaimAsync(user!, new Claim("IsAdmin", "1"));
            return NoContent();
        }

    }
}
