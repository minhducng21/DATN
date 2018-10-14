namespace CodeExam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        CommentDescription = c.String(),
                        CommentTime = c.DateTime(),
                        ParentId = c.Int(),
                        CommentStatus = c.Int(),
                    })
                .PrimaryKey(t => t.CommentId);
            
            CreateTable(
                "dbo.DataType",
                c => new
                    {
                        DataTypeId = c.Int(nullable: false, identity: true),
                        DataTypeName = c.String(maxLength: 50),
                        DataTypeStatus = c.Int(),
                    })
                .PrimaryKey(t => t.DataTypeId);
            
            CreateTable(
                "dbo.LanguageProgram",
                c => new
                    {
                        LanguageId = c.Int(nullable: false, identity: true),
                        LanguageName = c.String(maxLength: 50),
                        LanguageStatus = c.Int(),
                    })
                .PrimaryKey(t => t.LanguageId);
            
            CreateTable(
                "dbo.LeaderBoard",
                c => new
                    {
                        LeaderBoardId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        TaskId = c.Int(),
                        Point = c.Int(),
                        SourceCode = c.String(),
                        LanguageId = c.Int(),
                    })
                .PrimaryKey(t => t.LeaderBoardId);
            
            CreateTable(
                "dbo.RoleUser",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(maxLength: 50),
                        RoleStatus = c.Int(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Task",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        TaskName = c.String(),
                        TaskDescription = c.String(),
                        TaskLevel = c.String(maxLength: 20),
                        Point = c.Int(),
                        Input = c.String(),
                        OutputType = c.Int(),
                        TaskStatus = c.Int(),
                    })
                .PrimaryKey(t => t.TaskId);
            
            CreateTable(
                "dbo.TestCase",
                c => new
                    {
                        TestCaseId = c.Int(nullable: false, identity: true),
                        Input = c.String(),
                        Outut = c.String(),
                    })
                .PrimaryKey(t => t.TestCaseId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 50),
                        Password = c.String(maxLength: 50),
                        RoleId = c.Int(nullable: false),
                        Email = c.String(maxLength: 50),
                        UserStatus = c.Int(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.TestCase");
            DropTable("dbo.Task");
            DropTable("dbo.RoleUser");
            DropTable("dbo.LeaderBoard");
            DropTable("dbo.LanguageProgram");
            DropTable("dbo.DataType");
            DropTable("dbo.Comment");
        }
    }
}
