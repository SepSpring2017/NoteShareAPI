using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteShareAPI.Entities;
using NoteShareAPI.Models;

namespace NoteShareAPI.Controllers
{
    [Route("api/[controller]")]
    public class SubjectsController : Controller
    {
        private readonly NoteContext db;

        public SubjectsController(NoteContext context)
        {
            db = context;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<SubjectDTO> Get()
        {
            var subjects = db.Subjects.ToList().OrderBy(s => s.Name);
            var DTOList = new List<SubjectDTO>();
            foreach (var subject in subjects)
                DTOList.Add(new SubjectDTO(subject));
            return DTOList;
        }

        [HttpGet("search/{query}")]
        public IEnumerable<SubjectDTO> Search(string query)
        {
            query = query.ToLower();
            return Get().Where(s => s.name.ToLower().Contains(query) || s.subjectId.ToString().Contains(query));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var subject = db.Subjects.FirstOrDefault(s => s.SubjectId == id);
            if (subject != null)
                return Ok(new SubjectDTO(subject));
            return NotFound(new { message = "No subject found for that Id" });
        }

        // POST api/values
        [Authorize]
        [HttpPost]
        public void Post(Subject s)
        {
            var subject = new Subject
            {
                SubjectId = s.SubjectId,
                Name = s.Name
            };
            db.Subjects.Add(subject);
            db.SaveChanges();
        }

        // PUT api/values/5
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult Put(int id, Subject s)
        {
            var existingSubject = db.Subjects.FirstOrDefault(subject => subject.SubjectId == id);
            if (existingSubject != null)
            {
                existingSubject.Name = s.Name;
                db.SaveChanges();
                return Ok(new SubjectDTO(existingSubject));
            }
            return BadRequest(new { message = "No subject found for that Id" });
        }

        // DELETE api/values/5
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existingSubject = db.Subjects.FirstOrDefault(subject => subject.SubjectId == id);
            if (existingSubject != null)
            {
                db.Subjects.Remove(existingSubject);
                db.SaveChanges();
                return Ok();
            }
            return BadRequest(new { message = "No subject found for that Id" });
        }
    }
}
