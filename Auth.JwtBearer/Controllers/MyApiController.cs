using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace Auth.JwtBearer.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class MyApiController : ControllerBase
    {
        [HttpGet]
        [Route("get")]
        public IActionResult Get()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in User.Claims)
            {
                stringBuilder.Append($"{item.Type}-{item.Value} \r\n");
            }
            //返回上下文的Claims
            return Ok(stringBuilder.ToString());
        }
    }
}
