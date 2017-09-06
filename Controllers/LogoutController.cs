using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteShareAPI.Entities;

namespace NoteShareAPI.Controllers
{
    [Route("api/[controller]")]
    public class LogoutController : Controller
    {
        private readonly SignInManager<ApplicationUser> _manager;

        public LogoutController(SignInManager<ApplicationUser> manager)
        {
            _manager = manager;
        }

        [Authorize]
        [HttpGet]
        public ActionResult Get()
        {
            var result = _manager.SignOutAsync();
            if (result.IsCompletedSuccessfully)
                return Ok();
            return Unauthorized();
        }
    }
}