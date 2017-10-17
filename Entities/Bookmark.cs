using System;

namespace NoteShareAPI.Entities
{
    public class Bookmark
    {
        public Bookmark()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public Document Document { get; set; }
        public ApplicationUser User { get; set; }
    }
}
