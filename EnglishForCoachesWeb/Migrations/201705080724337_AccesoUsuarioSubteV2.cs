namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccesoUsuarioSubteV2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubTemaAccesoUsuario",
                c => new
                    {
                        SubTemaAccesoUsuarioId = c.Int(nullable: false, identity: true),
                        SubTemaId = c.Int(nullable: false),
                        FechaAcceso = c.DateTime(nullable: false),
                        AlumnoId = c.String(),
                    })
                .PrimaryKey(t => t.SubTemaAccesoUsuarioId)
                .ForeignKey("dbo.SubTema", t => t.SubTemaId, cascadeDelete: true)
                .Index(t => t.SubTemaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubTemaAccesoUsuario", "SubTemaId", "dbo.SubTema");
            DropIndex("dbo.SubTemaAccesoUsuario", new[] { "SubTemaId" });
            DropTable("dbo.SubTemaAccesoUsuario");
        }
    }
}
