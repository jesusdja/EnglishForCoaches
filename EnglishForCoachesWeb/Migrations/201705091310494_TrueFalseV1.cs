namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrueFalseV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrueFalse",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enunciado = c.String(),
                        UrlImagen = c.String(),
                        RespuestaCorrecta = c.Boolean(nullable: false),
                        Explicacion = c.String(nullable: false),
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
            DropForeignKey("dbo.TrueFalse", "TipoEjercicioId", "dbo.TipoEjercicio");
            DropForeignKey("dbo.TrueFalse", "SubTemaId", "dbo.SubTema");
            DropForeignKey("dbo.TrueFalse", "BloqueId", "dbo.Bloque");
            DropForeignKey("dbo.TrueFalse", "AreaId", "dbo.Area");
            DropIndex("dbo.TrueFalse", new[] { "BloqueId" });
            DropIndex("dbo.TrueFalse", new[] { "TipoEjercicioId" });
            DropIndex("dbo.TrueFalse", new[] { "AreaId" });
            DropIndex("dbo.TrueFalse", new[] { "SubTemaId" });
            DropTable("dbo.TrueFalse");
        }
    }
}
