using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace NoteShareAPI.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public List<Subject> subjects { get; set; }
    }
}
