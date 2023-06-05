using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace WebApi.Controllers
{
    public class UsersController : CrudController<User>
    {
        private IUsersService _service;

        public UsersController(IUsersService service) : base(service)
        {
            _service = service;
        }

        //wyłączamy metodę z routingu
        [NonAction]
        public override Task<IActionResult> Get()
        {
            return base.Get();
        }


        [HttpGet]
        public async Task<IActionResult> Get(string? username)
        {
            if(string.IsNullOrWhiteSpace(username))
            {
                return await Get();
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
    }
}
