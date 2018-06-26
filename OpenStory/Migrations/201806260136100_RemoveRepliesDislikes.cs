namespace OpenStory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRepliesDislikes : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Replies", "Dislikes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Replies", "Dislikes", c => c.Int(nullable: false));
        }
    }
}
