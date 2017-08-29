using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteShareAPI.Entities;

namespace NoteShareAPI.Controllers
{
    [Route("api/[controller]")]
    public class DocumentsController : Controller
    {
        private readonly NoteContext db;
        private readonly IHostingEnvironment env;

        public DocumentsController(NoteContext context, IHostingEnvironment host)
        {
            db = context;
            env = host;
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
        public ActionResult Post(IFormFile file)
        {
            try 
            {
                var uploadDirectory = $"{env.WebRootPath}/Uploads";
                if (!Directory.Exists(uploadDirectory))
                    Directory.CreateDirectory(uploadDirectory);
                if (file.Length > 0)
                {
                    var allFiles = Directory.GetFiles(uploadDirectory).ToList();
                    var fileName = Services.GetUniqueSlug(file.FileName, allFiles);
                    var filePath = Path.Combine(uploadDirectory, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyToAsync(stream);
                    }

                    var document = new Document
                    {
                        FileName = fileName,
                        DocumentType = file.ContentType
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
