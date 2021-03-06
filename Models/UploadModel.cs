using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace NoteShareAPI.Models
{
    public class UploadModel
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string DocumentName { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public string DocumentType { get; set; }
    }
}