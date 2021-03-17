namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class beatthegoalieV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BeatTheGoalie",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FicheroAudio = c.String(),
                        Respuesta1 = c.String(nullable: false),
                        Respuesta2 = c.String(nullable: false),
                        Respuesta3 = c.String(),
                        Respuesta4 = c.String(),
                        RespuestaCorrecta = c.Int(nullable: false),
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
            DropForeignKey("dbo.BeatTheGoalie", "TipoEjercicioId", "dbo.TipoEjercicio");
            DropForeignKey("dbo.BeatTheGoalie", "SubTemaId", "dbo.SubTema");
            DropForeignKey("dbo.BeatTheGoalie", "BloqueId", "dbo.Bloque");
            DropForeignKey("dbo.BeatTheGoalie", "AreaId", "dbo.Area");
            DropIndex("dbo.BeatTheGoalie", new[] { "BloqueId" });
            DropIndex("dbo.BeatTheGoalie", new[] { "TipoEjercicioId" });
            DropIndex("dbo.BeatTheGoalie", new[] { "AreaId" });
            DropIndex("dbo.BeatTheGoalie", new[] { "SubTemaId" });
            DropTable("dbo.BeatTheGoalie");
        }
    }
}
