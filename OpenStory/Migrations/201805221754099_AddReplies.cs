namespace OpenStory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReplies : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Replies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReplyDate = c.DateTime(nullable: false),
                        Content = c.String(),
                        Likes = c.Int(nullable: false),
                        Dislikes = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        Topic_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Topics", t => t.Topic_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Topic_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Replies", "Topic_Id", "dbo.Topics");
            DropForeignKey("dbo.Replies", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Replies", new[] { "Topic_Id" });
            DropIndex("dbo.Replies", new[] { "ApplicationUser_Id" });
            DropTable("dbo.Replies");
        }
    }
}
