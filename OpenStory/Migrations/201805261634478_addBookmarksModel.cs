namespace OpenStory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBookmarksModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookmarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        Topic_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Topics", t => t.Topic_Id)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.Topic_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bookmarks", "Topic_Id", "dbo.Topics");
            DropForeignKey("dbo.Bookmarks", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Bookmarks", new[] { "Topic_Id" });
            DropIndex("dbo.Bookmarks", new[] { "ApplicationUserId" });
            DropTable("dbo.Bookmarks");
        }
    }
}
