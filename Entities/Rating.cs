namespace NoteShareAPI.Entities
{
    public class Rating
    {
        public ApplicationUser User { get; set; }
        public Document Document { get; set; }
        public bool IsUpvote { get; set; }
    }
}
