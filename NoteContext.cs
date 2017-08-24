using Microsoft.EntityFrameworkCore;
using NoteShareAPI.Entities;

namespace NoteShareAPI
{
    public class NoteContext : DbContext
    {
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Document> Documents { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=NoteShare.db");
        }
    }
}