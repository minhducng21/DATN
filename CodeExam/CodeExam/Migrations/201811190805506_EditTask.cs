namespace CodeExam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditTask : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Task", "OutputType", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Task", "OutputType", c => c.Int());
        }
    }
}
