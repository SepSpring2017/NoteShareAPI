using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteShareAPI.Entities;

namespace NoteShareAPI.Controllers
{
    [Route("api/[controller]")]
    public class DocumentsController : Controller
    {
        private readonly NoteContext db = new NoteContext();

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
        public void Post(Document d)
        {
            var document = new Document
            {
                FileName = d.FileName,
                DocumentType = d.DocumentType
            };
            db.Documents.Add(document);
            db.SaveChanges();
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
