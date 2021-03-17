namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoticiaV3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GrupoUsuario", "Noticia_NoticiaId", "dbo.Noticia");
            DropIndex("dbo.GrupoUsuario", new[] { "Noticia_NoticiaId" });
            CreateIndex("dbo.NoticiaGrupo", "NoticiaId");
            AddForeignKey("dbo.NoticiaGrupo", "NoticiaId", "dbo.Noticia", "NoticiaId", cascadeDelete: true);
            DropColumn("dbo.GrupoUsuario", "Noticia_NoticiaId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GrupoUsuario", "Noticia_NoticiaId", c => c.Int());
            DropForeignKey("dbo.NoticiaGrupo", "NoticiaId", "dbo.Noticia");
            DropIndex("dbo.NoticiaGrupo", new[] { "NoticiaId" });
            CreateIndex("dbo.GrupoUsuario", "Noticia_NoticiaId");
            AddForeignKey("dbo.GrupoUsuario", "Noticia_NoticiaId", "dbo.Noticia", "NoticiaId");
        }
    }
}
