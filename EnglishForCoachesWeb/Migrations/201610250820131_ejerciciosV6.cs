namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ejerciciosV6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FillTheNext",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enunciado = c.String(),
                        Respuestas = c.String(),
                        Descripcion = c.String(),
                        SubTemaId = c.Int(nullable: false),
                        AreaId = c.Int(nullable: false),
                        TipoEjercicioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Area", t => t.AreaId, cascadeDelete: true)
                .ForeignKey("dbo.SubTema", t => t.SubTemaId, cascadeDelete: true)
                .ForeignKey("dbo.TipoEjercicio", t => t.TipoEjercicioId, cascadeDelete: true)
                .Index(t => t.SubTemaId)
                .Index(t => t.AreaId)
                .Index(t => t.TipoEjercicioId);
            
            CreateTable(
                "dbo.WordByWord",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enunciado = c.String(),
                        Descripcion = c.String(),
                        SubTemaId = c.Int(nullable: false),
                        AreaId = c.Int(nullable: false),
                        TipoEjercicioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Area", t => t.AreaId, cascadeDelete: true)
                .ForeignKey("dbo.SubTema", t => t.SubTemaId, cascadeDelete: true)
                .ForeignKey("dbo.TipoEjercicio", t => t.TipoEjercicioId, cascadeDelete: true)
                .Index(t => t.SubTemaId)
                .Index(t => t.AreaId)
                .Index(t => t.TipoEjercicioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WordByWord", "TipoEjercicioId", "dbo.TipoEjercicio");
            DropForeignKey("dbo.WordByWord", "SubTemaId", "dbo.SubTema");
            DropForeignKey("dbo.WordByWord", "AreaId", "dbo.Area");
            DropForeignKey("dbo.FillTheNext", "TipoEjercicioId", "dbo.TipoEjercicio");
            DropForeignKey("dbo.FillTheNext", "SubTemaId", "dbo.SubTema");
            DropForeignKey("dbo.FillTheNext", "AreaId", "dbo.Area");
            DropIndex("dbo.WordByWord", new[] { "TipoEjercicioId" });
            DropIndex("dbo.WordByWord", new[] { "AreaId" });
            DropIndex("dbo.WordByWord", new[] { "SubTemaId" });
            DropIndex("dbo.FillTheNext", new[] { "TipoEjercicioId" });
            DropIndex("dbo.FillTheNext", new[] { "AreaId" });
            DropIndex("dbo.FillTheNext", new[] { "SubTemaId" });
            DropTable("dbo.WordByWord");
            DropTable("dbo.FillTheNext");
        }
    }
}
