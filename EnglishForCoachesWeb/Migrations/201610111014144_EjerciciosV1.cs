namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EjerciciosV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Area",
                c => new
                    {
                        AreaId = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.AreaId);
            
            CreateTable(
                "dbo.Skillwise",
                c => new
                    {
                        SkillwiseId = c.Int(nullable: false, identity: true),
                        Enunciado = c.String(),
                        Respuesta1 = c.String(),
                        Respuesta2 = c.String(),
                        Respuesta3 = c.String(),
                        Respuesta4 = c.String(),
                        RespuestaCorrecta = c.Int(nullable: false),
                        Explicacion1 = c.String(),
                        Explicacion2 = c.String(),
                        Explicacion3 = c.String(),
                        Explicacion4 = c.String(),
                        Descripcion = c.String(),
                        TemaId = c.Int(nullable: false),
                        AreaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SkillwiseId)
                .ForeignKey("dbo.Area", t => t.AreaId, cascadeDelete: true)
                .ForeignKey("dbo.Tema", t => t.TemaId, cascadeDelete: true)
                .Index(t => t.TemaId)
                .Index(t => t.AreaId);
            
            CreateTable(
                "dbo.Tema",
                c => new
                    {
                        TemaId = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.TemaId);
            
            CreateTable(
                "dbo.SubTema",
                c => new
                    {
                        SubTemaId = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                        TemaId = c.Int(nullable: false),
                        Orden = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SubTemaId)
                .ForeignKey("dbo.Tema", t => t.TemaId, cascadeDelete: true)
                .Index(t => t.TemaId);
            
            CreateTable(
                "dbo.Test",
                c => new
                    {
                        TestId = c.Int(nullable: false, identity: true),
                        Enunciado = c.String(),
                        Respuesta1 = c.String(),
                        Respuesta2 = c.String(),
                        Respuesta3 = c.String(),
                        Respuesta4 = c.String(),
                        RespuestaCorrecta = c.Int(nullable: false),
                        Explicacion = c.String(),
                        Descripcion = c.String(),
                        TemaId = c.Int(nullable: false),
                        AreaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TestId)
                .ForeignKey("dbo.Area", t => t.AreaId, cascadeDelete: true)
                .ForeignKey("dbo.Tema", t => t.TemaId, cascadeDelete: true)
                .Index(t => t.TemaId)
                .Index(t => t.AreaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Test", "TemaId", "dbo.Tema");
            DropForeignKey("dbo.Test", "AreaId", "dbo.Area");
            DropForeignKey("dbo.SubTema", "TemaId", "dbo.Tema");
            DropForeignKey("dbo.Skillwise", "TemaId", "dbo.Tema");
            DropForeignKey("dbo.Skillwise", "AreaId", "dbo.Area");
            DropIndex("dbo.Test", new[] { "AreaId" });
            DropIndex("dbo.Test", new[] { "TemaId" });
            DropIndex("dbo.SubTema", new[] { "TemaId" });
            DropIndex("dbo.Skillwise", new[] { "AreaId" });
            DropIndex("dbo.Skillwise", new[] { "TemaId" });
            DropTable("dbo.Test");
            DropTable("dbo.SubTema");
            DropTable("dbo.Tema");
            DropTable("dbo.Skillwise");
            DropTable("dbo.Area");
        }
    }
}
