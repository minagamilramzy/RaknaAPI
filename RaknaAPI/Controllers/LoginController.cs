using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RaknaAPI.Data;
using RaknaAPI.Models.Auth;
using RaknaAPI.Models.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RaknaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly RaknaAPIContext _context;

        public LoginController(IConfiguration configuration, SignInManager<User> signInManager, UserManager<User> userManager, RaknaAPIContext context)
        {
            _configuration = configuration;
            this.signInManager = signInManager;
            this.userManager = userManager;
            _context = context;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login([FromBody] Userloginmodel Login)
        {
            IActionResult result = Unauthorized();
            var user = await authenticateuser(Login);
            if (user != null)
            {
                var tokenString = GenerateJSONWebtoken(User);
                result = Ok(new { Token = tokenString});      

            }
            return result;  

        }

        private object GenerateJSONWebtoken(ClaimsPrincipal user)
        {
            var key =new  SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));
            var credentitals = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var claimsIdendity = new[]
            {
                //new Claim("Name",User.Email),
                new Claim("TestClaim","anything you want")

            };
            var Token = new JwtSecurityToken(_configuration["jwt:Issuer"],

                _configuration["jwt:Issuer"],
                claims: claimsIdendity,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentitals);


            return new  JwtSecurityTokenHandler().WriteToken(Token);    

            }

        private async Task<User> authenticateuser(Userloginmodel login)
        {

            var result = await signInManager.PasswordSignInAsync(login.Email,login.Password,false,lockoutOnFailure:false);
            if (result.Succeeded)
            {
                var UserInfo = await userManager.FindByEmailAsync(login.Email);
                return UserInfo;

            }
            return null;
        
        }
    }
}
