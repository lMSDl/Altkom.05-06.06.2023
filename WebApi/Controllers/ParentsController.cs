using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Filters;

namespace WebApi.Controllers
{
    public class ParentsController : ApiController
    {

        [HttpGet]
        [ServiceFilter(typeof(LimitFilter))]
        public IActionResult Get()
        {
            var parent = new Parent();
            parent.Name = "Parent";
            parent.DateTime = DateTime.Now;

            var child = new Child() { Name = "Child1", Parent = parent };


            parent.Children = new List<Child>()
            {
                child,

                child,


                child,


                child,


                child,

                new Child() {Name = "Child2", Parent =parent}
            };

            return Ok(parent);
        }
    }
}
