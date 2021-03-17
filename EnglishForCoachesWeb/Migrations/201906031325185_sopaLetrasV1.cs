namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sopaLetrasV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CasillaSopaLetras",
                c => new
                    {
                        CasillaSopaLetrasId = c.Int(nullable: false, identity: true),
                        PosV = c.Int(nullable: false),
                        PosH = c.Int(nullable: false),
                        letra = c.String(),
                        SopaLetras_Id = c.Int(),
                    })
                .PrimaryKey(t => t.CasillaSopaLetrasId)
                .ForeignKey("dbo.SopaLetras", t => t.SopaLetras_Id)
                .Index(t => t.SopaLetras_Id);
            
            CreateTable(
                "dbo.SopaLetras",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enunciado = c.String(nullable: false),
                        Descripcion = c.String(),
                        SubTemaId = c.Int(nullable: false),
                        TipoJuegoOnlineId = c.Int(nullable: false),
                        JuegoOnlineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JuegoOnline", t => t.JuegoOnlineId, cascadeDelete: false)
                .ForeignKey("dbo.SubTema", t => t.SubTemaId, cascadeDelete: false)
                .ForeignKey("dbo.TipoJuegoOnline", t => t.TipoJuegoOnlineId, cascadeDelete: true)
                .Index(t => t.SubTemaId)
                .Index(t => t.TipoJuegoOnlineId)
                .Index(t => t.JuegoOnlineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SopaLetras", "TipoJuegoOnlineId", "dbo.TipoJuegoOnline");
            DropForeignKey("dbo.SopaLetras", "SubTemaId", "dbo.SubTema");
            DropForeignKey("dbo.SopaLetras", "JuegoOnlineId", "dbo.JuegoOnline");
            DropForeignKey("dbo.CasillaSopaLetras", "SopaLetras_Id", "dbo.SopaLetras");
            DropIndex("dbo.SopaLetras", new[] { "JuegoOnlineId" });
            DropIndex("dbo.SopaLetras", new[] { "TipoJuegoOnlineId" });
            DropIndex("dbo.SopaLetras", new[] { "SubTemaId" });
            DropIndex("dbo.CasillaSopaLetras", new[] { "SopaLetras_Id" });
            DropTable("dbo.SopaLetras");
            DropTable("dbo.CasillaSopaLetras");
        }
    }
}
