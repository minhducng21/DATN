namespace CodeExam.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CodeWarDbContext : DbContext
    {
        public CodeWarDbContext()
            : base("name=CodeWarDbContext")
        {
        }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DataType> DataTypes { get; set; }
        public DbSet<LanguageProgram> LanguagePrograms { get; set; }
        public DbSet<LeaderBoard> LeaderBoards { get; set; }
        public DbSet<RoleUser> RoleUsers { get; set; }
        public DbSet<TestCase> TestCases { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
