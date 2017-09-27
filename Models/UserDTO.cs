using System.Collections.Generic;
using System.Linq;
using NoteShareAPI.Entities;

namespace NoteShareAPI.Models
{
    public class UserDTO
    {
        public UserDTO(ApplicationUser u)
        {
            this.Email = u.Email;
            this.Id = u.Id;
            this.Subjects = u.subjects;
            this.Roles = new List<RoleDTO>();

            var db = new NoteContext();
            var userRoles = db.UserRoles.Where(r => r.UserId == u.Id).ToList().Select(r => r.RoleId);
            foreach (var r in db.Roles.Where(role => userRoles.Contains(role.Id)).ToList())
                this.Roles.Add(new RoleDTO(r));
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<RoleDTO> Roles { get; set; }
    }
}