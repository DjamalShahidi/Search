using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AmootSearch.Entities
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext() : base()
        {
        }
        public DataBaseContext(string connectionString) : base(GetOptions(connectionString))
        {
        }
        private static DbContextOptions GetOptions(string connectionString)
        {
            return new DbContextOptionsBuilder().UseSqlServer(connectionString).Options;
        }
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DatabaseInitializer.ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Model>()
                   .HasIndex(u => u.Index)
                   .IsUnique();
            builder.Entity<Synonym>()
                 .HasIndex(u => u.Main);
            builder.Entity<Synonym>()
                 .HasIndex(u => u.Equivalent);
        }

        public virtual DbSet<Text> Texts { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<Synonym> Synonyms { get; set; }
    }
}
