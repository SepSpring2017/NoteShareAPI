using System.Collections.Generic;
using System.Linq;
using NoteShareAPI.Entities;

namespace NoteShareAPI.Models
{
    public class UserDTO
    {
        public UserDTO(ApplicationUser u)
        {
            var db = new NoteContext();

            this.email = u.Email;
            this.id = u.Id;
            this.roles = new List<RoleDTO>();
            this.subjects = db.UserSubjects.Where(us => us.UserId == u.Id).Select(us => us.Subject).ToList();

            var userRoles = db.UserRoles.Where(r => r.UserId == u.Id).ToList().Select(r => r.RoleId);
            foreach (var r in db.Roles.Where(role => userRoles.Contains(role.Id)).ToList())
                this.roles.Add(new RoleDTO(r));
        }

        public UserDTO() {}

        public string id { get; set; }
        public string email { get; set; }
        public List<Subject> subjects { get; set; }
        public List<RoleDTO> roles { get; set; }
    }
}