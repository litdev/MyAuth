using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Auth.Policy.Controllers
{
    public class AuthorizationController : Controller
    {
        public IActionResult Login()
        {

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"张三"),
                new Claim("Address","这里是地址"),
                new Claim("Othersss","额外的喜喜"),
                new Claim(ClaimTypes.Role,"Admin"),//给了个角色
            };
            var identity = new ClaimsIdentity(claims, "TestIdentity");
            HttpContext.SignInAsync(new ClaimsPrincipal(identity));

            return View();
        }

        [Authorize]
        public IActionResult AccessDenied(string ReturnUrl = "")
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [Authorize]
        public IActionResult LoginOut()
        {
            HttpContext.SignOutAsync();
            return Content("");
        }

    }
}
