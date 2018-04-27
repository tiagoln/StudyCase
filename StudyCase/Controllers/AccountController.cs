using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Core.DTOs;
using Core.Interfaces;
using Core.Model;
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
        private readonly IUnityOfWork _uow;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            JwtTokenGenerator jwtTokenGenerator, 
            IUnityOfWork uow)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _uow = uow;
        }

        [HttpPost]
        public async Task<object> Login([FromBody] LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                return _jwtTokenGenerator.GenerateJwtToken(model.Email, appUser);
            }

            if (result.IsNotAllowed)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);

                if (appUser == null)
                {
                    return "This user do not exist!";
                }

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

            if (result.ToString().Equals("Failed"))
            {
                return "Wrong login or password!";
            }

            return "DAFUK?! oO";
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

            throw new ApplicationException("UNKNOWN_ERROR");
        }

        public class RegisterDto
        {
            [Required] public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
            public string Password { get; set; }
        }
    }
}