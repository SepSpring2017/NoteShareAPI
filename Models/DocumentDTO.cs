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
            documentName = d.DocumentName;
            documentType = d.DocumentType;
            subject = d.Subject;
            fileName = d.FileName;
            id = d.ID;
            uploadDate = d.UploadDate;
        }
    }
}