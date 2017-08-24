using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoteShareAPI.Entities;

namespace NoteShareAPI.Controllers
{
    [Route("api/[controller]")]
    public class SubjectsController : Controller
    {
        NoteContext db = new NoteContext();

        // GET api/values
        [HttpGet]
        public IEnumerable<Subject> Get()
        {
            return db.Subjects.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Subject Get(int id)
        {
            return db.Subjects.FirstOrDefault(s => s.SubjectId == id);
        }

        // POST api/values
        [HttpPost]
        public void Post(Subject s)
        {
            var subject = new Subject
            {
                SubjectId = s.SubjectId,
                Name = s.Name
            };
            db.Subjects.Add(subject);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
