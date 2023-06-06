using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Claims;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Authorize]
    public class UsersController : CrudController<User>
    {
        private IUsersService _service;
        private AuthService _authService;

        public UsersController(IUsersService service, AuthService authService) : base(service)
        {
            _service = service;
            _authService = authService;
        }

        //wyłączamy metodę z routingu
        [NonAction]
        public override Task<IActionResult> Get()
        {
            //opcjonlanie:
            return null!;
            //return base.Get();
        }



        [HttpGet]
        //potrzeba każda z wymienionych ról (sprawdzanie w osobnych filtrach)
        /*[Authorize(Roles = "UserAdmin")]
        [Authorize(Roles = "Read")]*/
        [Authorize(Roles = nameof(Roles.UserAdmin))]
        [Authorize(Roles = nameof(Roles.Read))]

        //potrzebna jedna z wymienionych ról
        //[Authorize(Roles = "Read, UserAdmin")]
        public async Task<IActionResult> Get(string? username)
        {
            if(string.IsNullOrWhiteSpace(username))
            {
                return await base.Get();
            }

            var user = await _service.FindByNameAsync(username);
            if (user == null)
                return NotFound();

            return Ok(user);
        }


        [HttpGet("{text:alpha:maxlength(10)}")]
        public string Text(string text)
        {
            return text;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(User user)
        {
            var token = await _authService.AuthAsync(user.Username, user.Password);
            if (token == null)
                return Unauthorized();


            //Generacja ciasteczka
            /*var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimPrincipal = new ClaimsPrincipal(claimIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);*/


            return Ok(token);
        }

        [AllowAnonymous]
        public override Task<IActionResult> Post(User entity)
        {
            return base.Post(entity);
        }
    }
}
