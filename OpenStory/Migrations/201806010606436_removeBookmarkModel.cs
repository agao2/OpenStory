namespace OpenStory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeBookmarkModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bookmarks", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Bookmarks", "Topic_Id", "dbo.Topics");
            DropIndex("dbo.Bookmarks", new[] { "ApplicationUserId" });
            DropIndex("dbo.Bookmarks", new[] { "Topic_Id" });
            DropTable("dbo.Bookmarks");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Bookmarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        Topic_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Bookmarks", "Topic_Id");
            CreateIndex("dbo.Bookmarks", "ApplicationUserId");
            AddForeignKey("dbo.Bookmarks", "Topic_Id", "dbo.Topics", "Id");
            AddForeignKey("dbo.Bookmarks", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
