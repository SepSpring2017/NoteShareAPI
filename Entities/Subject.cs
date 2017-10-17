namespace NoteShareAPI.Entities
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string Name { get; set; }


        public override bool Equals(object obj)
        {
            var item = obj as Subject;

            if (item == null)
                return false;
            
            return SubjectId == item.SubjectId;
        }

        public override int GetHashCode()
        {
            return SubjectId.GetHashCode();
        }
    }
}