namespace NoteShareAPI.Entities
{
    public class Rating
    {
        public string Id { get; set; }
        public ApplicationUser User { get; set; }
        public Document Document { get; set; }
        public bool IsUpvote { get; set; }
    }
}
