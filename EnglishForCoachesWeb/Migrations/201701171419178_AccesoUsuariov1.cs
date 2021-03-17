namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccesoUsuariov1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccesoUsuario",
                c => new
                    {
                        AccesoUsuarioId = c.Int(nullable: false, identity: true),
                        UsuarioId = c.String(),
                        Usuario = c.String(),
                        FechaAcceso = c.DateTime(nullable: false),
                        Ip = c.String(),
                    })
                .PrimaryKey(t => t.AccesoUsuarioId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AccesoUsuario");
        }
    }
}
