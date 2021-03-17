namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EjerciciosV3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MatchTheWord",
                c => new
                    {
                        MatchTheWordId = c.Int(nullable: false, identity: true),
                        Pregunta1 = c.String(),
                        Pregunta2 = c.String(),
                        Pregunta3 = c.String(),
                        Pregunta4 = c.String(),
                        Pregunta5 = c.String(),
                        Respuesta1 = c.String(),
                        Respuesta2 = c.String(),
                        Respuesta3 = c.String(),
                        Respuesta4 = c.String(),
                        Respuesta5 = c.String(),
                        Explicacion = c.String(),
                        Descripcion = c.String(),
                        SubTemaId = c.Int(nullable: false),
                        AreaId = c.Int(nullable: false),
                        TipoEjercicioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MatchTheWordId)
                .ForeignKey("dbo.Area", t => t.AreaId, cascadeDelete: true)
                .ForeignKey("dbo.SubTema", t => t.SubTemaId, cascadeDelete: true)
                .ForeignKey("dbo.TipoEjercicio", t => t.TipoEjercicioId, cascadeDelete: true)
                .Index(t => t.SubTemaId)
                .Index(t => t.AreaId)
                .Index(t => t.TipoEjercicioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MatchTheWord", "TipoEjercicioId", "dbo.TipoEjercicio");
            DropForeignKey("dbo.MatchTheWord", "SubTemaId", "dbo.SubTema");
            DropForeignKey("dbo.MatchTheWord", "AreaId", "dbo.Area");
            DropIndex("dbo.MatchTheWord", new[] { "TipoEjercicioId" });
            DropIndex("dbo.MatchTheWord", new[] { "AreaId" });
            DropIndex("dbo.MatchTheWord", new[] { "SubTemaId" });
            DropTable("dbo.MatchTheWord");
        }
    }
}
