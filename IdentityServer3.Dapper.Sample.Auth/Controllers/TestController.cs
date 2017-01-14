using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IdentityServer3.Dapper.Sample.Auth.Controllers
{
    public class TestController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok("Ping");
        }
    }
}
