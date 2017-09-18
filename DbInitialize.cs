
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
                    UserName = "test@test.com",
                    Email = "test@test.com"
                };
                _manager.CreateAsync(newUser, "J8cG!FjD");
            }

            if (!_context.Subjects.Any())
            {
                var scraper = new SubjectScraper(_context);
                scraper.Start();
            }

            _context.SaveChanges();
        }
    }
}