using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        private readonly NoteContext _db;

        public UsersController(UserManager<ApplicationUser> manager, NoteContext db)
        {
            _manager = manager;
            _db = db;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IEnumerable<UserDTO> Get()
        {
            var allUsers = _manager.Users.ToList();
            var DTOList = new List<UserDTO>();
            foreach (var user in allUsers)
                DTOList.Add(new UserDTO(user));
            return DTOList;
        }

        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var user = _manager.FindByIdAsync(id).Result;
            if (user != null)
                return Ok(new UserDTO(user));
            return NotFound(new { message = "No user found for that Id" });
        }

        [HttpGet("current")]
        public ActionResult GetCurrent()
        {
            var id = _manager.GetUserId(User);
            return Get(id);
        }

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
            {
                _manager.AddToRoleAsync(newUser, "Student");
                return Ok();
            }
            return BadRequest(new { Message = result.ToString() });
        }

        [HttpPut("{userId}")]
        public ActionResult Put(string userId, [FromBody] UserDTO u)
        {
            var user = _manager.FindByIdAsync(userId).Result;
            if (user == null)
                return BadRequest(new { message = "No user found for that Id" });
            user.Subjects = u.subjects;
            var result = _manager.UpdateAsync(user).Result;
            if (result.Succeeded)
                return Ok();
            return BadRequest(new { Message = result.ToString() });
        }

        [HttpPut("addsubject")]
        public async Task<ActionResult> Put(int subjectId)
        {
            var user = await _manager.GetUserAsync(User);
            if (user.Subjects == null)
                user.Subjects = new List<Subject>();

            var subject = _db.Subjects.FirstOrDefault(s => s.SubjectId == subjectId);
            Console.WriteLine(subject.Name);
            if (!user.Subjects.Contains(subject))
                user.Subjects.Add(subject);

            var result = await _manager.UpdateAsync(user);
            Console.WriteLine($"Count: {user.Subjects.Count}");
            if (result.Succeeded)
                return Ok(new UserDTO(user));
            return BadRequest(new { Message = result.ToString() });
        }

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
