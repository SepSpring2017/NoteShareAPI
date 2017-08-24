
using System.Collections.Generic;
using System.Linq;
using NoteShareAPI.Entities;

namespace NoteShareAPI
{
    public static class DbInitialize
    {
        public static void Seed(NoteContext context)
        {
            context.Database.EnsureCreated();

            if (context.Subjects.Any())
                return;

            var subjects = new List<Subject>
            {
                new Subject
                {
                    Name = "Software Engineering Practice",
                    SubjectId = 48440
                },
                new Subject
                {
                    Name = "Software Architecture",
                    SubjectId = 48433
                }
            };

            context.Subjects.AddRange(subjects);
            context.SaveChanges();
        }
    }
}