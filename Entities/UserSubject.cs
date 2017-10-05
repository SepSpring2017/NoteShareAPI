namespace NoteShareAPI.Entities
{
    public class UserSubject
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}