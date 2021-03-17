namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RespuestasExamenV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RespuestaIncorrecta",
                c => new
                    {
                        RespuestaIncorrectaId = c.Int(nullable: false, identity: true),
                        ExamenId = c.Int(nullable: false),
                        Pregunta = c.String(),
                        Respuesta = c.String(),
                    })
                .PrimaryKey(t => t.RespuestaIncorrectaId)
                .ForeignKey("dbo.Examen", t => t.ExamenId, cascadeDelete: true)
                .Index(t => t.ExamenId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RespuestaIncorrecta", "ExamenId", "dbo.Examen");
            DropIndex("dbo.RespuestaIncorrecta", new[] { "ExamenId" });
            DropTable("dbo.RespuestaIncorrecta");
        }
    }
}
