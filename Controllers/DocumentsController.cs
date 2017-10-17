using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteShareAPI.Entities;
using NoteShareAPI.Models;

namespace NoteShareAPI.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class DocumentsController : Controller
    {
        private readonly NoteContext _db;
        private readonly IHostingEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;

        public DocumentsController(NoteContext context, IHostingEnvironment host, UserManager<ApplicationUser> manager)
        {
            _db = context;
            _env = host;
            _userManager = manager;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<DocumentDTO> Get()
        {
            return _db.Documents.Include(d => d.Owner).Include(d => d.Subject).Select(d => new DocumentDTO(d)).ToList().OrderBy(d => d.documentName);
        }

        [HttpGet("search")]
        public IEnumerable<DocumentDTO> Search(string query)
        {
            if (query == null)
                query = string.Empty;
            query = query.ToLower();
            return Get().Where(d => d.documentName.ToLower().Contains(query) || d.documentType.ToLower().Contains(query));
        }

        [HttpGet("searchsubject")]
        public IEnumerable<DocumentDTO> SearchSubject(string subject, string query)
        {
            subject = subject.ToLower();
            return Search(query).Where(d => d.subject.Name.ToLower().Contains(subject) || d.subject.SubjectId.ToString().Contains(subject));
        }

        [HttpPost("bookmark")]
        public ActionResult BookmarkDocument(string id)
        {
            var document = _db.Documents.FirstOrDefault(s => s.Id == id);
            if (document == null)
                return NotFound(new { message = "No document found for that Id" });
            var user = _userManager.GetUserAsync(User).Result;

            if (_db.Bookmarks.Any(b => b.Document.Id == id && b.User.Id == user.Id))
                return BadRequest(new { message = "This bookmark already exists" });

            var bookmark = new Bookmark
            {
                User = user,
                Document = document
            };

            _db.Bookmarks.Add(bookmark);
            _db.SaveChanges();
            return Ok();
        }

        [HttpPost("vote")]
        public ActionResult Vote(string documentId, bool isUpvote)
        {
            var document = _db.Documents.FirstOrDefault(s => s.Id == documentId);
            if (document == null)
                return NotFound(new { message = "No document found for that Id" });

            var user = _userManager.GetUserAsync(User).Result;
            var rating = _db.Ratings.FirstOrDefault(r => r.User.Id == user.Id && r.Document.Id == documentId);

            if (rating == null)
            {
                var newRating = new Rating
                {
                    User = user,
                    Document = document,
                    IsUpvote = isUpvote
                };
                _db.Ratings.Add(newRating);
            }
            else
            {
                rating.IsUpvote = isUpvote;
            }

            _db.SaveChanges();
            return Ok();
        }

        [HttpPost("upvote")]
        public ActionResult UpVote(string documentId)
        {
            return Vote(documentId, true);
        }

        [HttpPost("downvote")]
        public ActionResult DownVote(string documentId)
        {
            return Vote(documentId, false);
        }

        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var document = _db.Documents.FirstOrDefault(s => s.Id == id);
            if (document != null)
                return Ok(new DocumentDTO(document));
            return NotFound(new { message = "No document found for that Id" });
        }

        [HttpPost]
        public async Task<ActionResult> Post(UploadModel upload)
        {
            if (!ModelState.IsValid)
            {
                var messages = ModelState.Values.Select(e => e.Errors.Select(error => error.ErrorMessage).First());
                return BadRequest(new { Message = String.Join(" ", messages) });
            }
            try
            {
                var uploadDirectory = $"{_env.WebRootPath}/Uploads";
                if (!Directory.Exists(uploadDirectory))
                    Directory.CreateDirectory(uploadDirectory);
                if (upload.File.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(upload.File.FileName);
                    var filePath = Path.Combine(uploadDirectory, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await upload.File.CopyToAsync(stream);
                    }

                    var subject = _db.Subjects.FirstOrDefault(s => s.SubjectId == upload.SubjectId);

                    var document = new Document
                    {
                        FileName = fileName,
                        DocumentName = upload.DocumentName,
                        DocumentType = upload.DocumentType,
                        Owner = _userManager.GetUserAsync(User).Result,
                        Subject = subject
                    };
                    _db.Documents.Add(document);
                    _db.SaveChanges();
                    return Ok(new DocumentDTO(document));
                }
                return BadRequest(new { message = "You need to upload a document" });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(string id, DocumentDTO d)
        {
            var existingDocument = _db.Documents.FirstOrDefault(document => document.Id == id);
            if (existingDocument != null)
            {
                existingDocument.DocumentType = d.documentType;
                existingDocument.DocumentName = d.documentName;
                _db.SaveChanges();
                return Ok(new DocumentDTO(existingDocument));
            }
            return BadRequest(new { message = "No document found for that Id" });
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var existingDocument = _db.Documents.FirstOrDefault(document => document.Id == id);
            if (existingDocument != null)
            {
                _db.Documents.Remove(existingDocument);
                _db.SaveChanges();
                return Ok();
            }
            return BadRequest(new { message = "No document found for that Id" });
        }
    }
}
