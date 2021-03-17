namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExamenDesbloqueadoV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExamenDesbloqueado",
                c => new
                    {
                        ExamenDesbloqueadoId = c.Int(nullable: false, identity: true),
                        SubTemaId = c.Int(nullable: false),
                        FechaDesbloqueo = c.DateTime(nullable: false),
                        AlumnoId = c.String(),
                        Examen_ExamenId = c.Int(),
                    })
                .PrimaryKey(t => t.ExamenDesbloqueadoId)
                .ForeignKey("dbo.Examen", t => t.Examen_ExamenId)
                .Index(t => t.Examen_ExamenId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExamenDesbloqueado", "Examen_ExamenId", "dbo.Examen");
            DropIndex("dbo.ExamenDesbloqueado", new[] { "Examen_ExamenId" });
            DropTable("dbo.ExamenDesbloqueado");
        }
    }
}
