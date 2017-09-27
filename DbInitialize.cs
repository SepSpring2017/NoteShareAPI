
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
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitialize(UserManager<ApplicationUser> manager, NoteContext context, RoleManager<IdentityRole> roleManager)
        {
            _manager = manager;
            _context = context;
            _roleManager = roleManager;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();

            string[] roles = {
                "Admin",
                "Moderator",
                "Student"
            };

            foreach (var role in roles)
            {
                if (!_roleManager.RoleExistsAsync(role).Result)
                {
                    var r = new IdentityRole();
                    r.Name = role;
                    _roleManager.CreateAsync(r);
                }
            }

            if (!_manager.Users.Any())
            {
                var newUser = new ApplicationUser
                {
                    UserName = "test@test.com",
                    Email = "test@test.com"
                };
                var result = _manager.CreateAsync(newUser, "J8cG!FjD").Result;
                if (result.Succeeded)
                    _manager.AddToRoleAsync(newUser, "Admin");
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