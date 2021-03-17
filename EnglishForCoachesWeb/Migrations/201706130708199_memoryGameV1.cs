namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class memoryGameV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MemoryGame",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UrlImagen = c.String(),
                        PalabraImagen = c.String(nullable: false, maxLength: 16),
                        Descripcion = c.String(),
                        SubTemaId = c.Int(nullable: false),
                        TipoJuegoOnlineId = c.Int(nullable: false),
                        JuegoOnlineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JuegoOnline", t => t.JuegoOnlineId, cascadeDelete: true)
                .ForeignKey("dbo.SubTema", t => t.SubTemaId, cascadeDelete: false)
                .ForeignKey("dbo.TipoJuegoOnline", t => t.TipoJuegoOnlineId, cascadeDelete: false)
                .Index(t => t.SubTemaId)
                .Index(t => t.TipoJuegoOnlineId)
                .Index(t => t.JuegoOnlineId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemoryGame", "TipoJuegoOnlineId", "dbo.TipoJuegoOnline");
            DropForeignKey("dbo.MemoryGame", "SubTemaId", "dbo.SubTema");
            DropForeignKey("dbo.MemoryGame", "JuegoOnlineId", "dbo.JuegoOnline");
            DropIndex("dbo.MemoryGame", new[] { "JuegoOnlineId" });
            DropIndex("dbo.MemoryGame", new[] { "TipoJuegoOnlineId" });
            DropIndex("dbo.MemoryGame", new[] { "SubTemaId" });
            DropTable("dbo.MemoryGame");
        }
    }
}
