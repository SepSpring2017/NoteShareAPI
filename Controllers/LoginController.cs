using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteShareAPI.Entities;

namespace NoteShareAPI.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly SignInManager<ApplicationUser> _manager;

        public LoginController(SignInManager<ApplicationUser> manager)
        {
            _manager = manager;
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post(string email, string password)
        {
            var result = _manager.PasswordSignInAsync(email, password, true, false);
            if (result.IsCompletedSuccessfully)
                return Ok();
            return Unauthorized();
        }
    }
}