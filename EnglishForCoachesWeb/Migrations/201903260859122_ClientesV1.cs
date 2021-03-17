namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientesV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClienteBloque",
                c => new
                    {
                        ClienteBloqueId = c.Int(nullable: false, identity: true),
                        ClienteId = c.Int(nullable: false),
                        BloqueId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClienteBloqueId);
            
            CreateTable(
                "dbo.ClienteExtra",
                c => new
                    {
                        ClienteExtraId = c.Int(nullable: false, identity: true),
                        ClienteId = c.Int(nullable: false),
                        ExtraId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClienteExtraId);
            
            CreateTable(
                "dbo.ClienteGramatica",
                c => new
                    {
                        ClienteGramaticaId = c.Int(nullable: false, identity: true),
                        ClienteId = c.Int(nullable: false),
                        GramaticaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClienteGramaticaId);
            
            CreateTable(
                "dbo.ClienteJuegoOnline",
                c => new
                    {
                        ClienteJuegoOnlineId = c.Int(nullable: false, identity: true),
                        ClienteId = c.Int(nullable: false),
                        JuegoOnlineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClienteJuegoOnlineId);
            
            CreateTable(
                "dbo.ClienteJuego",
                c => new
                    {
                        ClienteJuegoId = c.Int(nullable: false, identity: true),
                        ClienteId = c.Int(nullable: false),
                        JuegoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClienteJuegoId);
            
            CreateTable(
                "dbo.Cliente",
                c => new
                    {
                        ClienteId = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                        Abreviatura = c.String(),
                        NombreUrl = c.String(),
                        Logo = c.String(),
                        EstiloColor = c.String(),
                    })
                .PrimaryKey(t => t.ClienteId);
            
            CreateTable(
                "dbo.ClienteSubTema",
                c => new
                    {
                        ClienteSubTemaId = c.Int(nullable: false, identity: true),
                        ClienteId = c.Int(nullable: false),
                        SubTemaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClienteSubTemaId);
            
            CreateTable(
                "dbo.ClienteTema",
                c => new
                    {
                        ClienteTemaId = c.Int(nullable: false, identity: true),
                        ClienteId = c.Int(nullable: false),
                        TemaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClienteTemaId);
            
            CreateTable(
                "dbo.ClienteVideo",
                c => new
                    {
                        ClienteVideoId = c.Int(nullable: false, identity: true),
                        ClienteId = c.Int(nullable: false),
                        VideoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClienteVideoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ClienteVideo");
            DropTable("dbo.ClienteTema");
            DropTable("dbo.ClienteSubTema");
            DropTable("dbo.Cliente");
            DropTable("dbo.ClienteJuego");
            DropTable("dbo.ClienteJuegoOnline");
            DropTable("dbo.ClienteGramatica");
            DropTable("dbo.ClienteExtra");
            DropTable("dbo.ClienteBloque");
        }
    }
}
