namespace CodeExam.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfieldsocialId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "SocialId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "SocialId");
        }
    }
}
