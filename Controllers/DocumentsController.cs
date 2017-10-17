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
    [Route("api/[controller]")]
    public class DocumentsController : Controller
    {
        private readonly NoteContext db;
        private readonly IHostingEnvironment env;
        private readonly UserManager<ApplicationUser> userManager;

        public DocumentsController(NoteContext context, IHostingEnvironment host, UserManager<ApplicationUser> manager)
        {
            db = context;
            env = host;
            userManager = manager;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<DocumentDTO> Get()
        {
            return db.Documents.Include(d => d.Owner).Include(d => d.Subject).Select(d => new DocumentDTO(d)).ToList().OrderBy(d => d.documentName);
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

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("bookmark")]
        public ActionResult BookmarkDocument(string id)
        {
            var document = db.Documents.FirstOrDefault(s => s.ID == id);
            if (document == null)
                return NotFound(new { message = "No document found for that Id" });
            var user = userManager.GetUserAsync(User).Result;

            if (db.Bookmarks.Any(b => b.Document.ID == id && b.User.Id == user.Id))
                return BadRequest(new { message = "This bookmark already exists" });

            var bookmark = new Bookmark
            {
                User = user,
                Document = document
            };

            db.Bookmarks.Add(bookmark);
            db.SaveChanges();
            return Ok();
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("vote")]
        public ActionResult Vote(string documentId, bool isUpvote)
        {
            var document = db.Documents.FirstOrDefault(s => s.ID == documentId);
            if (document == null)
                return NotFound(new { message = "No document found for that Id" });

            var user = userManager.GetUserAsync(User).Result;
            var rating = db.Ratings.FirstOrDefault(r => r.User.Id == user.Id && r.Document.ID == documentId);

            if (rating == null)
            {
                var newRating = new Rating
                {
                    User = user,
                    Document = document,
                    IsUpvote = isUpvote
                };
                db.Ratings.Add(newRating);
            }
            else
            {
                rating.IsUpvote = isUpvote;
            }

            db.SaveChanges();
            return Ok();
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("upvote")]
        public ActionResult UpVote(string documentId)
        {
            return Vote(documentId, true);
        }

        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [HttpPost("downvote")]
        public ActionResult DownVote(string documentId)
        {
            return Vote(documentId, false);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var document = db.Documents.FirstOrDefault(s => s.ID == id);
            if (document != null)
                return Ok(new DocumentDTO(document));
            return NotFound(new { message = "No document found for that Id" });
        }

        // POST api/values
        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
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
                var uploadDirectory = $"{env.WebRootPath}/Uploads";
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

                    var subject = db.Subjects.FirstOrDefault(s => s.SubjectId == upload.SubjectId);

                    var document = new Document
                    {
                        FileName = fileName,
                        DocumentName = upload.DocumentName,
                        DocumentType = upload.DocumentType,
                        Owner = userManager.GetUserAsync(User).Result,
                        Subject = subject
                    };
                    db.Documents.Add(document);
                    db.SaveChanges();
                    return Ok(new DocumentDTO(document));
                }
                return BadRequest(new { message = "You need to upload a document" });
            } catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // PUT api/values/5
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult Put(string id, DocumentDTO d)
        {
            var existingDocument = db.Documents.FirstOrDefault(document => document.ID == id);
            if (existingDocument != null)
            {
                existingDocument.DocumentType = d.documentType;
                existingDocument.DocumentName = d.documentName;
                db.SaveChanges();
                return Ok(new DocumentDTO(existingDocument));
            }
            return BadRequest(new { message = "No document found for that Id" });
        }

        // DELETE api/values/5
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var existingDocument = db.Documents.FirstOrDefault(document => document.ID == id);
            if (existingDocument != null)
            {
                db.Documents.Remove(existingDocument);
                db.SaveChanges();
                return Ok();
            }
            return BadRequest(new { message = "No document found for that Id" });
        }
    }
}
