namespace CodeExam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActiveCodeUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ActiveCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ActiveCode");
        }
    }
}
