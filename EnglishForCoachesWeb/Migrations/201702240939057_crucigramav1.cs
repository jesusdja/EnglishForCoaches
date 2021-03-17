namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class crucigramav1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CasillaCrucigrama",
                c => new
                    {
                        CasillaCrucigramaId = c.Int(nullable: false, identity: true),
                        CrucigramaId = c.Int(nullable: false),
                        PosV = c.Int(nullable: false),
                        PosH = c.Int(nullable: false),
                        letra = c.String(),
                    })
                .PrimaryKey(t => t.CasillaCrucigramaId);
            
            CreateTable(
                "dbo.Crucigrama",
                c => new
                    {
                        CrucigramaId = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false),
                        Horizontales = c.String(nullable: false),
                        Verticales = c.String(nullable: false),
                        SubTemaId = c.Int(nullable: false),
                        CategoriaJuegoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CrucigramaId)
                .ForeignKey("dbo.CategoriaJuego", t => t.CategoriaJuegoId, cascadeDelete: true)
                .ForeignKey("dbo.SubTema", t => t.SubTemaId, cascadeDelete: true)
                .Index(t => t.SubTemaId)
                .Index(t => t.CategoriaJuegoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Crucigrama", "SubTemaId", "dbo.SubTema");
            DropForeignKey("dbo.Crucigrama", "CategoriaJuegoId", "dbo.CategoriaJuego");
            DropIndex("dbo.Crucigrama", new[] { "CategoriaJuegoId" });
            DropIndex("dbo.Crucigrama", new[] { "SubTemaId" });
            DropTable("dbo.Crucigrama");
            DropTable("dbo.CasillaCrucigrama");
        }
    }
}
