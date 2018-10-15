namespace CodeExam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfielddatatype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataType", "DisplayName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DataType", "DisplayName");
        }
    }
}
