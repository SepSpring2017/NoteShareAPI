using System;
using NoteShareAPI.Entities;

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

        public DocumentDTO(Document d)
        {
            this.documentName = d.DocumentName;
            this.documentType = d.DocumentType;
            this.subject = d.Subject;
            this.fileName = d.FileName;
            this.id = d.ID;
            this.uploadDate = d.UploadDate;
        }
    }
}