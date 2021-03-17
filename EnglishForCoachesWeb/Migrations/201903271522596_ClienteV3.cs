namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClienteV3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClienteSubTemaVideo",
                c => new
                    {
                        ClienteSubTemaVideoId = c.Int(nullable: false, identity: true),
                        ClienteId = c.Int(nullable: false),
                        SubTemaVideoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClienteSubTemaVideoId);
            
            DropTable("dbo.ClienteVideo");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ClienteVideo",
                c => new
                    {
                        ClienteVideoId = c.Int(nullable: false, identity: true),
                        ClienteId = c.Int(nullable: false),
                        VideoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClienteVideoId);
            
            DropTable("dbo.ClienteSubTemaVideo");
        }
    }
}
