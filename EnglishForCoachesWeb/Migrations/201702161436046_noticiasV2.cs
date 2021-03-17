namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class noticiasV2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Noticia", "GrupoUsuarioId", "dbo.GrupoUsuario");
            DropIndex("dbo.Noticia", new[] { "GrupoUsuarioId" });
            AddColumn("dbo.GrupoUsuario", "Noticia_NoticiaId", c => c.Int());
            CreateIndex("dbo.GrupoUsuario", "Noticia_NoticiaId");
            AddForeignKey("dbo.GrupoUsuario", "Noticia_NoticiaId", "dbo.Noticia", "NoticiaId");
            DropColumn("dbo.Noticia", "GrupoUsuarioId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Noticia", "GrupoUsuarioId", c => c.Int(nullable: false));
            DropForeignKey("dbo.GrupoUsuario", "Noticia_NoticiaId", "dbo.Noticia");
            DropIndex("dbo.GrupoUsuario", new[] { "Noticia_NoticiaId" });
            DropColumn("dbo.GrupoUsuario", "Noticia_NoticiaId");
            CreateIndex("dbo.Noticia", "GrupoUsuarioId");
            AddForeignKey("dbo.Noticia", "GrupoUsuarioId", "dbo.GrupoUsuario", "GrupoUsuarioId", cascadeDelete: true);
        }
    }
}
