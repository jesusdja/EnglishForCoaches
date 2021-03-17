namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class noticiasV11 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Noticia", "GrupoUsuarioId");
            AddForeignKey("dbo.Noticia", "GrupoUsuarioId", "dbo.GrupoUsuario", "GrupoUsuarioId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Noticia", "GrupoUsuarioId", "dbo.GrupoUsuario");
            DropIndex("dbo.Noticia", new[] { "GrupoUsuarioId" });
        }
    }
}
