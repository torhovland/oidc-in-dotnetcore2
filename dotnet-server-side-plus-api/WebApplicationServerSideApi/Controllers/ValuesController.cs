using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var user = HttpContext.User;
            var claims = user.Claims.ToList();

            return new[]
            {
                $"Namn: {user.Identity.Name}",
                $"Brukarnamn (subject ID): {claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value}",
                $"Epostadresse: {claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value}",
                $"Favorittfarge: {claims.FirstOrDefault(c => c.Type == "favorittfarge")?.Value}"
            };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
