using Microsoft.AspNetCore.Identity;

namespace NoteShareAPI.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // Removed custom properties because many to many is broken in EF Core
    }
}
