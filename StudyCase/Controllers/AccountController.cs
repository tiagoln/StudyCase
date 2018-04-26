﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
            IConfiguration configuration,
            JwtTokenGenerator jwtTokenGenerator
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
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
                    return "This user do not exists!";
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
                return "Login attempt failed!";
            }

            return "DAFUK?! oO";
        }

        [HttpPost]
        public async Task<object> Register([FromBody] RegisterDto model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                UserProfile = new UserProfile
                {
                    Orders = new List<Order>(),                    
                }
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return "User created successfully, please login.";
            }

            throw new ApplicationException("UNKNOWN_ERROR");
        }

        public class LoginDto
        {
            [Required] public string Email { get; set; }

            [Required] public string Password { get; set; }
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