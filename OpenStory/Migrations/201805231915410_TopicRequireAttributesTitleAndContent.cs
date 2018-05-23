namespace OpenStory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TopicRequireAttributesTitleAndContent : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Topics", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Topics", "Content", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Topics", "Content", c => c.String());
            AlterColumn("dbo.Topics", "Title", c => c.String());
        }
    }
}
