using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using System.Net;

namespace WebApi.Controllers
{
    public abstract class CrudController<T> : ApiController where T : Entity
    {
        private readonly ICrudService<T> _service;

        public CrudController(ICrudService<T> service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get()
        {
            var entities = await _service.ReadAsync();
            return Ok(entities);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _service.ReadAsync(id);
            if (entity == null)
            {
                //return StatusCode(StatusCodes.Status404NotFound);
                return NotFound();
            }

            return Ok(entity);
        }

        [HttpDelete("{id:int:min(1)}")]
        public virtual async Task<IActionResult> Delete(int id)
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
        public async Task<IActionResult> Put(int id, /*[FromBody]*/T entity)
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
        public async Task<IActionResult> Post(T entity)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(entity.Id < 0)
            {
                ModelState.AddModelError(nameof(entity.Id), "Co to za id?");
                ModelState.AddModelError(nameof(entity.Id), "Czy musi być na -?");
                return BadRequest(ModelState);
            }


            entity = await _service.CreateAsync(entity);

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }
    }
}
