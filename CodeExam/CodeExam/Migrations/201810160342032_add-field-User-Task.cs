namespace CodeExam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfieldUserTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Task", "TestCaseId", c => c.Int(nullable: false));
            AddColumn("dbo.TestCase", "Output", c => c.String());
            AddColumn("dbo.Users", "DisplayName", c => c.String());
            DropColumn("dbo.TestCase", "Outut");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TestCase", "Outut", c => c.String());
            DropColumn("dbo.Users", "DisplayName");
            DropColumn("dbo.TestCase", "Output");
            DropColumn("dbo.Task", "TestCaseId");
        }
    }
}
