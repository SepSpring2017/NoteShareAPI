using Microsoft.AspNetCore.Identity;

namespace NoteShareAPI.Models
{
    public class RoleDTO
    {
        public RoleDTO(IdentityRole role)
        {
            Id = role.Id;
            Name = role.Name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}