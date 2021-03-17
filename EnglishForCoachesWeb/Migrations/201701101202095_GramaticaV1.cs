namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GramaticaV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Gramatica",
                c => new
                    {
                        GramaticaId = c.Int(nullable: false, identity: true),
                        Titulo = c.String(),
                        Descripcion = c.String(),
                        SubTemaId = c.Int(nullable: false),
                        Orden = c.Int(nullable: false),
                        AreaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GramaticaId)
                .ForeignKey("dbo.Area", t => t.AreaId, cascadeDelete: true)
                .ForeignKey("dbo.SubTema", t => t.SubTemaId, cascadeDelete: true)
                .Index(t => t.SubTemaId)
                .Index(t => t.AreaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Gramatica", "SubTemaId", "dbo.SubTema");
            DropForeignKey("dbo.Gramatica", "AreaId", "dbo.Area");
            DropIndex("dbo.Gramatica", new[] { "AreaId" });
            DropIndex("dbo.Gramatica", new[] { "SubTemaId" });
            DropTable("dbo.Gramatica");
        }
    }
}
