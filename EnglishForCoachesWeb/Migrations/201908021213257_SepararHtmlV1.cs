namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SepararHtmlV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GramaticaCuerpo",
                c => new
                    {
                        GramaticaCuerpoId = c.Int(nullable: false, identity: true),
                        Cuerpo = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.GramaticaCuerpoId);
            
            CreateTable(
                "dbo.ExtraExplicacion",
                c => new
                    {
                        ExtraExplicacionId = c.Int(nullable: false, identity: true),
                        Explicacion = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ExtraExplicacionId);
            
            AddColumn("dbo.Gramatica", "GramaticaCuerpoId", c => c.Int());
            AddColumn("dbo.Extra", "ExtraExplicacionId", c => c.Int());
            CreateIndex("dbo.Gramatica", "GramaticaCuerpoId");
            CreateIndex("dbo.Extra", "ExtraExplicacionId");
            AddForeignKey("dbo.Gramatica", "GramaticaCuerpoId", "dbo.GramaticaCuerpo", "GramaticaCuerpoId");
            AddForeignKey("dbo.Extra", "ExtraExplicacionId", "dbo.ExtraExplicacion", "ExtraExplicacionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Extra", "ExtraExplicacionId", "dbo.ExtraExplicacion");
            DropForeignKey("dbo.Gramatica", "GramaticaCuerpoId", "dbo.GramaticaCuerpo");
            DropIndex("dbo.Extra", new[] { "ExtraExplicacionId" });
            DropIndex("dbo.Gramatica", new[] { "GramaticaCuerpoId" });
            DropColumn("dbo.Extra", "ExtraExplicacionId");
            DropColumn("dbo.Gramatica", "GramaticaCuerpoId");
            DropTable("dbo.ExtraExplicacion");
            DropTable("dbo.GramaticaCuerpo");
        }
    }
}
