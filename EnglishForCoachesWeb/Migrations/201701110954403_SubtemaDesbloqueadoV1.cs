namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubtemaDesbloqueadoV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubTemaDesbloqueado",
                c => new
                    {
                        SubTemaDesbloqueadoId = c.Int(nullable: false, identity: true),
                        SubTemaId = c.Int(nullable: false),
                        ApplicationUser_Id = c.Int(nullable: false),
                        FechaDesbloqueo = c.DateTime(nullable: false),
                        ApplicationUser_Id1 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SubTemaDesbloqueadoId)
                .ForeignKey("dbo.SubTema", t => t.SubTemaId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id1)
                .Index(t => t.SubTemaId)
                .Index(t => t.ApplicationUser_Id1);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubTemaDesbloqueado", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubTemaDesbloqueado", "SubTemaId", "dbo.SubTema");
            DropIndex("dbo.SubTemaDesbloqueado", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.SubTemaDesbloqueado", new[] { "SubTemaId" });
            DropTable("dbo.SubTemaDesbloqueado");
        }
    }
}
