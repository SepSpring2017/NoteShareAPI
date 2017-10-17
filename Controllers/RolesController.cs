using System.Collections.Generic;
using System.Linq;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteShareAPI.Entities;
using NoteShareAPI.Models;

namespace NoteShareAPI.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Roles = "Admin")]
    [Route("api/[controller]")]
    public class RolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IEnumerable<RoleDTO> Get()
        {
            return _roleManager.Roles.Select(r => new RoleDTO(r)).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var role = _roleManager.FindByIdAsync(id).Result;
            if (role != null)
                return Ok(new RoleDTO(role));
            return NotFound(new { message = "No role found for that Id" });
        }

        [HttpPost]
        public ActionResult Post(string Name)
        {
            if (_roleManager.RoleExistsAsync(Name).Result)
                return BadRequest(new { message = "This role already exists" });
            _roleManager.CreateAsync(new IdentityRole(Name));
            return Ok();
        }

        [HttpPut]
        public ActionResult Put(RoleDTO role)
        {
            var existingRole = _roleManager.FindByIdAsync(role.Id).Result;
            if (existingRole == null)
                return NotFound(new { message = "No role found for that Id" });
            existingRole.Name = role.Name;
            _roleManager.UpdateAsync(existingRole);
            return Ok();
        }

        [HttpPost("addrole")]
        public ActionResult AddUserToRole(string userId, string role)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
                return NotFound(new { message = "No user found for that Id" });

            var result = _userManager.AddToRoleAsync(user, role).Result;
            if (result.Succeeded)
                return Ok();
            return BadRequest(new { message = result.Errors.Select(e => e.Description) });
        }

        [HttpPost("removerole")]
        public ActionResult RemoveUserFromRole(string userId, string role)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
                return NotFound(new { message = "No user found for that Id" });

            var result = _userManager.RemoveFromRoleAsync(user, role).Result;
            if (result.Succeeded)
                return Ok();
            return BadRequest(new { message = result.Errors.Select(e => e.Description) });
        }
    }
}