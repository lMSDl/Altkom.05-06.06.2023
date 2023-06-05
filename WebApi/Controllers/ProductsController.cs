﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using System.Net;

namespace WebApi.Controllers
{
    public class ProductsController : CrudController<Product>
    {

        public ProductsController(ICrudService<Product> service) : base(service)
        {
        }

        [NonAction]
        public override Task<IActionResult> Delete(int id)
        {
            return base.Delete(id);
        }
    }
}