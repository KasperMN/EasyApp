using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EasyApp.api.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductController : ApiController
    {

        // GET api/products
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/products/5
        [HttpGet, Route("{productTitle}")]
        public string Get(string productTitle)
        {
            return "value";
        }

        // POST api/products
        [HttpPost]
        public void Post([FromBody] JObject data)
        {
           
        }

        // PUT api/products/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/products/5
        public void Delete(int id)
        {
        }
    }
}
