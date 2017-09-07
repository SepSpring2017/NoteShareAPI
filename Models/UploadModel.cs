using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using NoteShareAPI.Entities;

namespace NoteShareAPI.Models
{
    public class UploadModel
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string DocumentName { get; set; }
        [Required]
        public Subject Subject { get; set; }
        [Required]
        public string DocumentType { get; set; }
    }
}