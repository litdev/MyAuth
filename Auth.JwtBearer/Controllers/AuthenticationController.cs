using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Auth.JwtBearer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly JwtTokenManagement _jwtTokenManagement;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            IOptions<JwtTokenManagement> options)
        {
            _logger = logger;
            _jwtTokenManagement = options.Value;
        }

        [AllowAnonymous]
        [HttpPost, Route("login")]
        public ActionResult Login([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request");
            }

            //下面签发JwtToken

            //用户名密码校验成功

            var claims = new[]
            {
                new Claim("SubId","10001"),
                new Claim("LoginName",request.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(_jwtTokenManagement.Issuer,
                _jwtTokenManagement.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_jwtTokenManagement.AccessExpiration),
                signingCredentials: credentials);
            
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return Ok(token);

        }
    }
}
