using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NoteShareAPI.Entities;

namespace NoteShareAPI.Models
{
    public class UserDTO
    {
        public UserDTO(ApplicationUser u)
        {
            var db = new NoteContext();

            email = u.Email;
            id = u.Id;
            subjects = db.UserSubjects.Where(us => us.UserId == u.Id).Select(us => new SubjectDTO(us.Subject)).ToList();
            bookmarks = db.Bookmarks.Include(b => b.Document.Subject).Where(b => b.User.Id == u.Id).Select(b => b.Document).Select(d => new DocumentDTO(d)).ToList();
            notes = db.Documents.Include(d => d.Subject).Where(d => d.OwnerId == u.Id).Select(d => new DocumentDTO(d)).ToList();

            roles = new List<RoleDTO>();
            var userRoles = db.UserRoles.Where(r => r.UserId == u.Id).ToList().Select(r => r.RoleId);
            foreach (var r in db.Roles.Where(role => userRoles.Contains(role.Id)).ToList())
                roles.Add(new RoleDTO(r));
        }

        public UserDTO() { }

        public string id { get; set; }
        public string email { get; set; }
        public List<SubjectDTO> subjects { get; set; }
        public List<RoleDTO> roles { get; set; }
        public List<DocumentDTO> bookmarks { get; set; }
        public List<DocumentDTO> notes { get; set; }
    }
}