namespace CodeExam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateRelaTaskTestcase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestCase", "TaskId", c => c.Int(nullable: false));
            DropColumn("dbo.Task", "TestCaseId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Task", "TestCaseId", c => c.Int(nullable: false));
            DropColumn("dbo.TestCase", "TaskId");
        }
    }
}
