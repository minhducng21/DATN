namespace CodeExam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditTaskLederBoard : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LeaderBoard", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.LeaderBoard", "TaskId", c => c.Int(nullable: false));
            AlterColumn("dbo.LeaderBoard", "Point", c => c.Int(nullable: false));
            AlterColumn("dbo.LeaderBoard", "LanguageId", c => c.Int(nullable: false));
            AlterColumn("dbo.Task", "Point", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Task", "Point", c => c.Int());
            AlterColumn("dbo.LeaderBoard", "LanguageId", c => c.Int());
            AlterColumn("dbo.LeaderBoard", "Point", c => c.Int());
            AlterColumn("dbo.LeaderBoard", "TaskId", c => c.Int());
            AlterColumn("dbo.LeaderBoard", "UserId", c => c.Int());
        }
    }
}
