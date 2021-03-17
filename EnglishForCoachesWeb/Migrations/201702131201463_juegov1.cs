namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class juegov1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoriaJuego",
                c => new
                    {
                        CategoriaJuegoId = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                        Icono = c.String(),
                    })
                .PrimaryKey(t => t.CategoriaJuegoId);
            
            CreateTable(
                "dbo.Juego",
                c => new
                    {
                        JuegoId = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false),
                        Explicacion = c.String(nullable: false),
                        Fichero = c.String(),
                        SubTemaId = c.Int(nullable: false),
                        CategoriaJuegoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JuegoId)
                .ForeignKey("dbo.CategoriaJuego", t => t.CategoriaJuegoId, cascadeDelete: true)
                .ForeignKey("dbo.SubTema", t => t.SubTemaId, cascadeDelete: true)
                .Index(t => t.SubTemaId)
                .Index(t => t.CategoriaJuegoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Juego", "SubTemaId", "dbo.SubTema");
            DropForeignKey("dbo.Juego", "CategoriaJuegoId", "dbo.CategoriaJuego");
            DropIndex("dbo.Juego", new[] { "CategoriaJuegoId" });
            DropIndex("dbo.Juego", new[] { "SubTemaId" });
            DropTable("dbo.Juego");
            DropTable("dbo.CategoriaJuego");
        }
    }
}
