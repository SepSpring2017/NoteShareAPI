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
                return Ok(new UserDTO(newUser));
            }
            return BadRequest(new { Message = result.ToString() });
        }

        [HttpPut("addsubject")]
        public async Task<ActionResult> Put(int subjectId)
        {
            var user = await _manager.GetUserAsync(User);
            var subjects = _db.UserSubjects.Where(us => us.UserId == user.Id).Select(us => us.Subject).ToList();
            Console.WriteLine($"{user.Email} has {subjects.Count} subjects before");

            var subject = _db.Subjects.FirstOrDefault(s => s.SubjectId == subjectId);
            if (!subjects.Contains(subject))
                _db.UserSubjects.Add(new UserSubject { User = user, Subject = subject });

            var result = _db.SaveChanges();
            subjects = _db.UserSubjects.Where(us => us.UserId == user.Id).Select(us => us.Subject).ToList();
            Console.WriteLine($"{user.Email} has {subjects.Count} subjects after");
            if (result > 0)
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
