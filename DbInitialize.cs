
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using NoteShareAPI.Entities;

namespace NoteShareAPI
{
    public class DbInitialize : IDbInitialize
    {
        private readonly UserManager<ApplicationUser> _manager;
        private readonly NoteContext _context;

        public DbInitialize(UserManager<ApplicationUser> manager, NoteContext context)
        {
            _manager = manager;
            _context = context;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();

            if (!_manager.Users.Any())
            {
                var newUser = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = "mhcodner@gmail.com"
                };
                _manager.CreateAsync(newUser, "J8cG!FjD");
            }

            if (!_context.Subjects.Any())
            {
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

                _context.Subjects.AddRange(subjects);
            }

            _context.SaveChanges();
        }
    }
}