using System.Linq;
using NoteShareAPI.Entities;

namespace NoteShareAPI.Models
{
    public class SubjectDTO
    {
        public int subjectId { get; set; }
        public string name { get; set; }
        public int documentCount { get; set; }

        public SubjectDTO(Subject s)
        {
            this.subjectId = s.SubjectId;
            this.name = s.Name;

            var db = new NoteContext();
            this.documentCount = db.Documents.Where(d => d.SubjectId == s.SubjectId).ToList().Count;
        }
    }
}