using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace NoteShareAPI.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public List<Subject> Subjects { get; set; }
        [InverseProperty("Owner")]
        public List<Document> Documents { get; set; }
    }
}
