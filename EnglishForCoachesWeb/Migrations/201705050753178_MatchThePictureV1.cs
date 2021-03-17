namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MatchThePictureV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MatchThePicture",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UrlImagen = c.String(nullable: false),
                        PalabraImagen = c.String(nullable: false, maxLength: 40),
                        Descripcion = c.String(),
                        SubTemaId = c.Int(nullable: false),
                        AreaId = c.Int(nullable: false),
                        TipoEjercicioId = c.Int(nullable: false),
                        BloqueId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Area", t => t.AreaId, cascadeDelete: true)
                .ForeignKey("dbo.Bloque", t => t.BloqueId, cascadeDelete: true)
                .ForeignKey("dbo.SubTema", t => t.SubTemaId, cascadeDelete: true)
                .ForeignKey("dbo.TipoEjercicio", t => t.TipoEjercicioId, cascadeDelete: true)
                .Index(t => t.SubTemaId)
                .Index(t => t.AreaId)
                .Index(t => t.TipoEjercicioId)
                .Index(t => t.BloqueId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MatchThePicture", "TipoEjercicioId", "dbo.TipoEjercicio");
            DropForeignKey("dbo.MatchThePicture", "SubTemaId", "dbo.SubTema");
            DropForeignKey("dbo.MatchThePicture", "BloqueId", "dbo.Bloque");
            DropForeignKey("dbo.MatchThePicture", "AreaId", "dbo.Area");
            DropIndex("dbo.MatchThePicture", new[] { "BloqueId" });
            DropIndex("dbo.MatchThePicture", new[] { "TipoEjercicioId" });
            DropIndex("dbo.MatchThePicture", new[] { "AreaId" });
            DropIndex("dbo.MatchThePicture", new[] { "SubTemaId" });
            DropTable("dbo.MatchThePicture");
        }
    }
}
