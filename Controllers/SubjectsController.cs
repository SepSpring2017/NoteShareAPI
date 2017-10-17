using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteShareAPI.Entities;
using NoteShareAPI.Models;

namespace NoteShareAPI.Controllers
{
    [Route("api/[controller]")]
    public class SubjectsController : Controller
    {
        private readonly NoteContext _db;

        public SubjectsController(NoteContext context)
        {
            _db = context;
        }
        
        [HttpGet]
        public IEnumerable<SubjectDTO> Get()
        {
            return _db.Subjects.Select(s => new SubjectDTO(s)).ToList().OrderBy(s => s.name);
        }

        [HttpGet("withnotes")]
        public IEnumerable<SubjectDTO> GetWithNotes()
        {
            return _db.Subjects.Select(s => new SubjectDTO(s)).ToList().Where(s => s.documentCount > 0).OrderBy(s => s.name);
        }

        [HttpGet("search/{query}")]
        public IEnumerable<SubjectDTO> Search(string query)
        {
            query = query.ToLower();
            return Get().Where(s => s.name.ToLower().Contains(query) || s.subjectId.ToString().Contains(query));
        }
        
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var subject = _db.Subjects.FirstOrDefault(s => s.SubjectId == id);
            if (subject != null)
                return Ok(new SubjectDTO(subject));
            return NotFound(new { message = "No subject found for that Id" });
        }
        
        [Authorize]
        [HttpPost]
        public void Post(Subject s)
        {
            var subject = new Subject
            {
                SubjectId = s.SubjectId,
                Name = s.Name
            };
            _db.Subjects.Add(subject);
            _db.SaveChanges();
        }
        
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult Put(int id, Subject s)
        {
            var existingSubject = _db.Subjects.FirstOrDefault(subject => subject.SubjectId == id);
            if (existingSubject != null)
            {
                existingSubject.Name = s.Name;
                _db.SaveChanges();
                return Ok(new SubjectDTO(existingSubject));
            }
            return BadRequest(new { message = "No subject found for that Id" });
        }
        
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existingSubject = _db.Subjects.FirstOrDefault(subject => subject.SubjectId == id);
            if (existingSubject != null)
            {
                _db.Subjects.Remove(existingSubject);
                _db.SaveChanges();
                return Ok();
            }
            return BadRequest(new { message = "No subject found for that Id" });
        }
    }
}
