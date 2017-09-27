using Microsoft.AspNetCore.Identity;

namespace NoteShareAPI.Models
{
    public class RoleDTO
    {
        public RoleDTO(IdentityRole role)
        {
            this.Id = role.Id;
            this.Name = role.Name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}