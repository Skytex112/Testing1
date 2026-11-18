using Microsoft.EntityFrameworkCore;
using ProjectTestingNote.Models;
using System.Diagnostics.CodeAnalysis;

namespace ProjectTestingNote.Data
{
    public class DataBaseContext : DbContext
    {
        
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        
        public DbSet<Note> Notes => Set<Note>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Note
            modelBuilder.Entity<Note>();

            base.OnModelCreating(modelBuilder);
        }


        //protected AppDbContext()
        //{
        //}
    }
}
