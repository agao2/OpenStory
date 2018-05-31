namespace OpenStory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeReplyInfoFromTopic : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Topics", "Content");
            DropColumn("dbo.Topics", "Dislikes");
            DropColumn("dbo.Topics", "Likes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Topics", "Likes", c => c.Int(nullable: false));
            AddColumn("dbo.Topics", "Dislikes", c => c.Int(nullable: false));
            AddColumn("dbo.Topics", "Content", c => c.String(nullable: false));
        }
    }
}
