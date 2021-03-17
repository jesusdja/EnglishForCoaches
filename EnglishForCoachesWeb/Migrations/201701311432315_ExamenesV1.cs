namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExamenesV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Examen",
                c => new
                    {
                        ExamenId = c.Int(nullable: false, identity: true),
                        BloqueId = c.Int(nullable: false),
                        AlumnoId = c.String(),
                        Aciertos = c.Int(nullable: false),
                        Fallos = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                        Aprobado = c.Boolean(nullable: false),
                        FechaExamen = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ExamenId)
                .ForeignKey("dbo.Bloque", t => t.BloqueId, cascadeDelete: true)
                .Index(t => t.BloqueId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Examen", "BloqueId", "dbo.Bloque");
            DropIndex("dbo.Examen", new[] { "BloqueId" });
            DropTable("dbo.Examen");
        }
    }
}
