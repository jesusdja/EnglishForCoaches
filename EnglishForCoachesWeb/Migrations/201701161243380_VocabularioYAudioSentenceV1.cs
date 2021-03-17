namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VocabularioYAudioSentenceV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AudioSentenceExercise",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enunciado = c.String(),
                        Respuestas = c.String(nullable: false),
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
            
            CreateTable(
                "dbo.Vocabulario",
                c => new
                    {
                        VocabularioId = c.Int(nullable: false, identity: true),
                        Palabra_es = c.String(),
                        Palabra_en = c.String(),
                        Definicion = c.String(),
                        FicheroAudio = c.String(),
                    })
                .PrimaryKey(t => t.VocabularioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AudioSentenceExercise", "TipoEjercicioId", "dbo.TipoEjercicio");
            DropForeignKey("dbo.AudioSentenceExercise", "SubTemaId", "dbo.SubTema");
            DropForeignKey("dbo.AudioSentenceExercise", "BloqueId", "dbo.Bloque");
            DropForeignKey("dbo.AudioSentenceExercise", "AreaId", "dbo.Area");
            DropIndex("dbo.AudioSentenceExercise", new[] { "BloqueId" });
            DropIndex("dbo.AudioSentenceExercise", new[] { "TipoEjercicioId" });
            DropIndex("dbo.AudioSentenceExercise", new[] { "AreaId" });
            DropIndex("dbo.AudioSentenceExercise", new[] { "SubTemaId" });
            DropTable("dbo.Vocabulario");
            DropTable("dbo.AudioSentenceExercise");
        }
    }
}
