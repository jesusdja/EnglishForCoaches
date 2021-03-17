namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bloqueDesbloqueadoV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BloqueDesbloqueado",
                c => new
                    {
                        BloqueDesbloqueadoId = c.Int(nullable: false, identity: true),
                        BloqueId = c.Int(nullable: false),
                        FechaDesbloqueo = c.DateTime(nullable: false),
                        AlumnoId = c.String(),
                    })
                .PrimaryKey(t => t.BloqueDesbloqueadoId)
                .ForeignKey("dbo.Bloque", t => t.BloqueId, cascadeDelete: true)
                .Index(t => t.BloqueId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BloqueDesbloqueado", "BloqueId", "dbo.Bloque");
            DropIndex("dbo.BloqueDesbloqueado", new[] { "BloqueId" });
            DropTable("dbo.BloqueDesbloqueado");
        }
    }
}
