using System;
using NoteShareAPI.Entities;
using System.Linq;

namespace NoteShareAPI.Models
{
    public class DocumentDTO
    {
        public string documentName { get; set; }
        public string documentType { get; set; }
        public Subject subject { get; set; }
        public string fileName { get; set; }
        public string id { get; set; }
        public DateTime uploadDate { get; set; }
        public double rating { get; set; }

        public DocumentDTO(Document d)
        {
            documentName = d.DocumentName;
            documentType = d.DocumentType;
            subject = d.Subject;
            fileName = d.FileName;
            id = d.ID;
            uploadDate = d.UploadDate;

            var db = new NoteContext();
            var allRatings = db.Ratings.Where(r => r.Document.ID == d.ID);
            if (allRatings.Count() == 0)
            {
                rating = 0.0;
            }
            else
            {
                rating = (double) allRatings.Count() / db.Ratings.Where(r => r.IsUpvote).Count();
            }
        }
    }
}