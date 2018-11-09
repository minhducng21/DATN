namespace CodeExam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuthorizeCtrl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControllerActions",
                c => new
                    {
                        CtrlId = c.Int(nullable: false, identity: true),
                        Ctrl = c.String(),
                        Action = c.String(),
                        Area = c.String(),
                        Description = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CtrlId);
            
            CreateTable(
                "dbo.RoleControllers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        CtrlId = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RoleControllers");
            DropTable("dbo.ControllerActions");
        }
    }
}
