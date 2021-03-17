namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Forov1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ForoCategoria",
                c => new
                    {
                        ForoCategoriaId = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.ForoCategoriaId);
            
            CreateTable(
                "dbo.ForoHilo",
                c => new
                    {
                        ForoHiloId = c.Int(nullable: false, identity: true),
                        Titulo = c.String(),
                        FechaCreacion = c.DateTime(nullable: false),
                        AlumnoId = c.String(),
                        ForoCategoriaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ForoHiloId)
                .ForeignKey("dbo.ForoCategoria", t => t.ForoCategoriaId, cascadeDelete: true)
                .Index(t => t.ForoCategoriaId);
            
            CreateTable(
                "dbo.ForoMensaje",
                c => new
                    {
                        ForoMensajeId = c.Int(nullable: false, identity: true),
                        Texto = c.String(),
                        FechaCreacion = c.DateTime(nullable: false),
                        AlumnoId = c.String(),
                        ForoHiloId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ForoMensajeId)
                .ForeignKey("dbo.ForoHilo", t => t.ForoHiloId, cascadeDelete: true)
                .Index(t => t.ForoHiloId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ForoMensaje", "ForoHiloId", "dbo.ForoHilo");
            DropForeignKey("dbo.ForoHilo", "ForoCategoriaId", "dbo.ForoCategoria");
            DropIndex("dbo.ForoMensaje", new[] { "ForoHiloId" });
            DropIndex("dbo.ForoHilo", new[] { "ForoCategoriaId" });
            DropTable("dbo.ForoMensaje");
            DropTable("dbo.ForoHilo");
            DropTable("dbo.ForoCategoria");
        }
    }
}
