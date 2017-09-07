using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<Document> Get()
        {
            return db.Documents.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var document = db.Documents.FirstOrDefault(s => s.ID == id);
            if (document != null)
                return Ok(document);
            return NotFound(new { message = "No document found for that Id" });
        }

        // POST api/values
        [Authorize]
        [HttpPost]
        public ActionResult Post([FromBody] UploadModel upload)
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
                    var allFiles = Directory.GetFiles(uploadDirectory).ToList();
                    var fileName = Services.GetUniqueSlug(upload.File.FileName, allFiles);
                    var filePath = Path.Combine(uploadDirectory, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        upload.File.CopyToAsync(stream);
                    }

                    var subject = db.Subjects.FirstOrDefault(s => s.SubjectId == upload.Subject.SubjectId);

                    var document = new Document
                    {
                        FileName = fileName,
                        DocumentName = upload.DocumentName,
                        DocumentType = upload.DocumentType,
                        Owner = userManager.FindByNameAsync(User.Identity.Name).Result,
                        Subject = subject
                    };
                    db.Documents.Add(document);
                    db.SaveChanges();
                    return Ok(new { file = $"/Uploads/{fileName}"});
                }
                return BadRequest();
            } catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // PUT api/values/5
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult Put(string id, Document d)
        {
            var existingDocument = db.Documents.FirstOrDefault(document => document.ID == id);
            if (existingDocument != null)
            {
                existingDocument.DocumentType = d.DocumentType;
                db.SaveChanges();
                return Ok(existingDocument);
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
