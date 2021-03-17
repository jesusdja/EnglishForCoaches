namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtraDesbloqueadoV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExtraDesbloqueado",
                c => new
                    {
                        ExtraDesbloqueadoId = c.Int(nullable: false, identity: true),
                        ExtraId = c.Int(nullable: false),
                        FechaDesbloqueo = c.DateTime(nullable: false),
                        AlumnoId = c.String(),
                    })
                .PrimaryKey(t => t.ExtraDesbloqueadoId)
                .ForeignKey("dbo.Extra", t => t.ExtraId, cascadeDelete: true)
                .Index(t => t.ExtraId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExtraDesbloqueado", "ExtraId", "dbo.Extra");
            DropIndex("dbo.ExtraDesbloqueado", new[] { "ExtraId" });
            DropTable("dbo.ExtraDesbloqueado");
        }
    }
}
