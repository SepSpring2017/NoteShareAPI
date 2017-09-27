using System.Collections.Generic;
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
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public List<Subject> Subjects { get; set; }
    }
}