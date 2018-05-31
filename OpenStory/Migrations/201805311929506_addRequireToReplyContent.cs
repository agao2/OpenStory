namespace OpenStory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRequireToReplyContent : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Replies", "Content", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Replies", "Content", c => c.String());
        }
    }
}
