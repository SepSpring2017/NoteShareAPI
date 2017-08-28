using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteShareAPI.Entities;

namespace NoteShareAPI.Controllers
{
    [Authorize]
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
        [Authorize]
        [HttpPost]
        public ActionResult Post(string email, string password)
        {
            var newUser = new ApplicationUser
            {
                UserName = email,
                Email = email
            };
            var result = _manager.CreateAsync(newUser, password);
            if (result.IsCompletedSuccessfully)
                return Ok();
            return BadRequest(result.ToString());
        }

        // PUT api/values/5
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult Put(string userId, List<Subject> subjects)
        {
            var user = _manager.FindByIdAsync(userId).Result;
            if (user == null)
                return BadRequest(new { message = "No user found for that Id" });
            user.subjects = subjects;
            var result = _manager.UpdateAsync(user);
            if (result.IsCompletedSuccessfully)
                return Ok();
            return BadRequest(result.ToString());
        }

        // DELETE api/values/5
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var user = _manager.FindByIdAsync(id).Result;
            if (user == null)
                return BadRequest(new { message = "No user found for that Id" });
            var result = _manager.DeleteAsync(user);
            if (result.IsCompletedSuccessfully)
                return Ok();
            return BadRequest();
        }
    }
}
