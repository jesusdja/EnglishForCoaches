namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AhorcadoV2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ahorcado",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Texto = c.String(nullable: false),
                        UrlImagen = c.String(),
                        Audio = c.String(),
                        Respuesta = c.String(),
                        Descripcion = c.String(),
                        SubTemaId = c.Int(nullable: false),
                        TipoJuegoOnlineId = c.Int(nullable: false),
                        JuegoOnlineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JuegoOnline", t => t.JuegoOnlineId, cascadeDelete: false)
                .ForeignKey("dbo.SubTema", t => t.SubTemaId, cascadeDelete: false)
                .ForeignKey("dbo.TipoJuegoOnline", t => t.TipoJuegoOnlineId, cascadeDelete: false)
                .Index(t => t.SubTemaId)
                .Index(t => t.TipoJuegoOnlineId)
                .Index(t => t.JuegoOnlineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ahorcado", "TipoJuegoOnlineId", "dbo.TipoJuegoOnline");
            DropForeignKey("dbo.Ahorcado", "SubTemaId", "dbo.SubTema");
            DropForeignKey("dbo.Ahorcado", "JuegoOnlineId", "dbo.JuegoOnline");
            DropIndex("dbo.Ahorcado", new[] { "JuegoOnlineId" });
            DropIndex("dbo.Ahorcado", new[] { "TipoJuegoOnlineId" });
            DropIndex("dbo.Ahorcado", new[] { "SubTemaId" });
            DropTable("dbo.Ahorcado");
        }
    }
}
