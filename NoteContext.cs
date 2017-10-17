using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoteShareAPI.Entities;

namespace NoteShareAPI
{
    public class NoteContext : IdentityDbContext<ApplicationUser>
    {
        public NoteContext(DbContextOptions options)
            : base(options) { }
        
        public NoteContext(){ }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<UserSubject> UserSubjects { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=NoteShare.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserSubject>()
                .HasKey(us => new { us.UserId, us.SubjectId });

            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

    }
}