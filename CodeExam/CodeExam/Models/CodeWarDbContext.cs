namespace CodeExam
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

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<DataType> DataTypes { get; set; }
        public virtual DbSet<LanguageProgram> LanguagePrograms { get; set; }
        public virtual DbSet<LeaderBoard> LeaderBoards { get; set; }
        public virtual DbSet<RoleUser> RoleUsers { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<TestCase> TestCases { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
