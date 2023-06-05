using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using System.Net;

namespace WebApi.Controllers
{
    public class UsersController : ApiController
    {
        private readonly ICrudService<User> _service;

        public UsersController(ICrudService<User> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _service.ReadAsync();
            return Ok(users);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _service.ReadAsync(id);
            if (user == null)
            {
                //return StatusCode(StatusCodes.Status404NotFound);
                return NotFound();
            }

            return Ok(user);
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> Delete(int id)
        {
            var localEntity = await _service.ReadAsync(id);
            if (localEntity == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(id);
            return NoContent();
        }


        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> Put(int id, /*[FromBody]*/User entity)
        {
            var localEntity = await _service.ReadAsync(id);
            if (localEntity == null)
            {
                return NotFound();
            }

            await _service.UpdateAsync(id, entity);
            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> Post(User entity)
        {
            entity = await _service.CreateAsync(entity);

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }



        [HttpGet("{text:alpha:maxlength(10)}")]
        public string Get(string text)
        {
            return text;
        }
    }
}
