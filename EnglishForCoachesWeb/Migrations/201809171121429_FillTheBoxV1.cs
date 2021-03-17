namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FillTheBoxV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FillTheBox",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titulo = c.String(),
                        Enunciado = c.String(),
                        Respuestas = c.String(nullable: false),
                        Explicacion = c.String(nullable: false),
                        FicheroAudio = c.String(),
                        Descripcion = c.String(),
                        SubTemaId = c.Int(nullable: false),
                        AreaId = c.Int(nullable: false),
                        TipoEjercicioId = c.Int(nullable: false),
                        BloqueId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Area", t => t.AreaId, cascadeDelete: true)
                .ForeignKey("dbo.Bloque", t => t.BloqueId, cascadeDelete: true)
                .ForeignKey("dbo.SubTema", t => t.SubTemaId, cascadeDelete: true)
                .ForeignKey("dbo.TipoEjercicio", t => t.TipoEjercicioId, cascadeDelete: true)
                .Index(t => t.SubTemaId)
                .Index(t => t.AreaId)
                .Index(t => t.TipoEjercicioId)
                .Index(t => t.BloqueId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FillTheBox", "TipoEjercicioId", "dbo.TipoEjercicio");
            DropForeignKey("dbo.FillTheBox", "SubTemaId", "dbo.SubTema");
            DropForeignKey("dbo.FillTheBox", "BloqueId", "dbo.Bloque");
            DropForeignKey("dbo.FillTheBox", "AreaId", "dbo.Area");
            DropIndex("dbo.FillTheBox", new[] { "BloqueId" });
            DropIndex("dbo.FillTheBox", new[] { "TipoEjercicioId" });
            DropIndex("dbo.FillTheBox", new[] { "AreaId" });
            DropIndex("dbo.FillTheBox", new[] { "SubTemaId" });
            DropTable("dbo.FillTheBox");
        }
    }
}
