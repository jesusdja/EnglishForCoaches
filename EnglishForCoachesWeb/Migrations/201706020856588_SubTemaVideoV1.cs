namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubTemaVideoV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubTemaVideo",
                c => new
                    {
                        SubTemaVideoId = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false),
                        UrlVideo = c.String(),
                        SubTemaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SubTemaVideoId)
                .ForeignKey("dbo.SubTema", t => t.SubTemaId, cascadeDelete: true)
                .Index(t => t.SubTemaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubTemaVideo", "SubTemaId", "dbo.SubTema");
            DropIndex("dbo.SubTemaVideo", new[] { "SubTemaId" });
            DropTable("dbo.SubTemaVideo");
        }
    }
}
