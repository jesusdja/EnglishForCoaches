namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BloqueRealizadosv1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BloqueRealizado",
                c => new
                    {
                        BloqueRealizadoId = c.Int(nullable: false, identity: true),
                        BloqueId = c.Int(nullable: false),
                        FechaRealizado = c.DateTime(nullable: false),
                        AlumnoId = c.String(),
                    })
                .PrimaryKey(t => t.BloqueRealizadoId)
                .ForeignKey("dbo.Bloque", t => t.BloqueId, cascadeDelete: true)
                .Index(t => t.BloqueId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BloqueRealizado", "BloqueId", "dbo.Bloque");
            DropIndex("dbo.BloqueRealizado", new[] { "BloqueId" });
            DropTable("dbo.BloqueRealizado");
        }
    }
}
