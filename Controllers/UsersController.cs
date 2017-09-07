using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteShareAPI.Entities;
using NoteShareAPI.Models;

namespace NoteShareAPI.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _manager;

        public UsersController(UserManager<ApplicationUser> manager)
        {
            _manager = manager;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<ApplicationUser> Get()
        {
            return _manager.Users.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var user = _manager.FindByIdAsync(id).Result;
            if (user != null)
                return Ok(user);
            return NotFound(new { message = "No user found for that Id" });
        }

        // POST api/values
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Post([FromBody] Credentials credentials)
        {
            if (!ModelState.IsValid)
            {
                var messages = ModelState.Values.Select(e => e.Errors.Select(error => error.ErrorMessage).First());
                return BadRequest(new { Message = String.Join(" ", messages) });
            }
            var newUser = new ApplicationUser
            {
                UserName = credentials.Email,
                Email = credentials.Email
            };
            var result = _manager.CreateAsync(newUser, credentials.Password).Result;
            if (result.Succeeded)
                return Ok();
            return BadRequest(new { Message = result.ToString() });
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public ActionResult Put(string userId, List<Subject> subjects)
        {
            var user = _manager.FindByIdAsync(userId).Result;
            if (user == null)
                return BadRequest(new { message = "No user found for that Id" });
            user.subjects = subjects;
            var result = _manager.UpdateAsync(user).Result;
            if (result.Succeeded)
                return Ok();
            return BadRequest(new { Message = result.ToString() });
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var user = _manager.FindByIdAsync(id).Result;
            if (user == null)
                return BadRequest(new { message = "No user found for that Id" });
            var result = _manager.DeleteAsync(user).Result;
            if (result.Succeeded)
                return Ok();
            return BadRequest();
        }
    }
}
