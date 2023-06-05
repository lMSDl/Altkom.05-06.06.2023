using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

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
        public Task<IEnumerable<User>> Get()
        {
            return _service.ReadAsync();
        }

        [HttpGet("{id:int:min(1)}")]
        public Task<User?> Get(int id)
        {
            return _service.ReadAsync(id);
        }

        [HttpDelete("{id:int:min(1)}")]
        public Task Delete(int id)
        {
            return _service.DeleteAsync(id);
        }


        [HttpPut("{id:int:min(1)}")]
        public Task Put(int id, /*[FromBody]*/User entity)
        {
            return _service.UpdateAsync(id, entity);
        }


        [HttpPost]
        public Task<User> Post(User entity)
        {
            return _service.CreateAsync(entity);
        }




        [HttpGet("{text:alpha:maxlength(10)}")]
        public string Get(string text)
        {
            return text;
        }
    }
}
