using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs;
using Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyCase.Services;

namespace StudyCase.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            JwtTokenGenerator jwtTokenGenerator,
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost]
        public async Task<object> Login([FromBody] LoginDto model)
        {
            var appUser = await _userManager.FindByEmailAsync(model.Email);

            if (appUser == null)
            {
                return "This user do not exist!";
            }

            var result = await _signInManager.CheckPasswordSignInAsync(appUser, model.Password, true);

            if (result.Succeeded)
            {
                return await _jwtTokenGenerator.GenerateJwtToken(model.Email, appUser);
            }

            if (result.IsNotAllowed)
            {
                if (!appUser.EmailConfirmed)
                {
                    return "You need to confirm your email!";
                }

                return "Can't login for some reason!";
            }

            if (result.IsLockedOut)
            {
                return "Too many attempts, try again later!";
            }

            return "Wrong login or password!";
        }

        [HttpPost]
        public async Task<object> Register([FromBody] RegisterDto model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return "User successfully created, please login.";
            }

            var sb = new StringBuilder();
            foreach (var error in result.Errors)
            {
                sb.AppendLine($"Error code {error.Code}: {error.Description}");
            }

            return sb.ToString();
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> MakeAdmin()
        {
            var userName = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            var user = await _userManager.FindByEmailAsync(userName);
            await _userManager.AddToRoleAsync(user, "Administrator");

            return Json(new
            {
                User.Identity.IsAuthenticated,
                user.Id,
                Name = $"{user.Email} {user.CreatedAt}",
                Type = User.Identity.AuthenticationType,
            });
        }
    }
}