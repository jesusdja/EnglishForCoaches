namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentoV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documento",
                c => new
                    {
                        DocumentoId = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false),
                        Descripcion = c.String(nullable: false),
                        FicheroAdjunto = c.String(),
                        SubTemaId = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentoId)
                .ForeignKey("dbo.SubTema", t => t.SubTemaId, cascadeDelete: true)
                .Index(t => t.SubTemaId);
            
            CreateTable(
                "dbo.DocumentoGrupo",
                c => new
                    {
                        DocumentoGrupoId = c.Int(nullable: false, identity: true),
                        DocumentoId = c.Int(nullable: false),
                        GrupoUsuarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentoGrupoId)
                .ForeignKey("dbo.Documento", t => t.DocumentoId, cascadeDelete: true)
                .ForeignKey("dbo.GrupoUsuario", t => t.GrupoUsuarioId, cascadeDelete: true)
                .Index(t => t.DocumentoId)
                .Index(t => t.GrupoUsuarioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documento", "SubTemaId", "dbo.SubTema");
            DropForeignKey("dbo.DocumentoGrupo", "GrupoUsuarioId", "dbo.GrupoUsuario");
            DropForeignKey("dbo.DocumentoGrupo", "DocumentoId", "dbo.Documento");
            DropIndex("dbo.DocumentoGrupo", new[] { "GrupoUsuarioId" });
            DropIndex("dbo.DocumentoGrupo", new[] { "DocumentoId" });
            DropIndex("dbo.Documento", new[] { "SubTemaId" });
            DropTable("dbo.DocumentoGrupo");
            DropTable("dbo.Documento");
        }
    }
}
