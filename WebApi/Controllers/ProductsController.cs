    using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using System.Net;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ServiceFilter(typeof(ConsoleLogFilter))]
    [Authorize]
    public class ProductsController : CrudController<Product>
    {

        public ProductsController(ICrudService<Product> service) : base(service)
        {
        }

        //[Authorize(Roles = "Create")]
        [Authorize(Roles = nameof(Roles.Create))]
        public override Task<IActionResult> Post(Product entity)
        {
            return base.Post(entity);
        }

        [NonAction]
        public override Task<IActionResult> Delete(int id)
        {
            return base.Delete(id);
        }
    }
}
