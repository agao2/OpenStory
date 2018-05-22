namespace OpenStory.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTopics : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostDate = c.DateTime(nullable: false),
                        Title = c.String(),
                        Content = c.String(),
                        Dislikes = c.Int(nullable: false),
                        Likes = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Topics", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Topics", new[] { "ApplicationUser_Id" });
            DropTable("dbo.Topics");
        }
    }
}
