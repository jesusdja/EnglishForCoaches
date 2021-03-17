namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class noticiasV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Noticia",
                c => new
                    {
                        NoticiaId = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false),
                        Texto = c.String(nullable: false),
                        Foto = c.String(),
                        FicheroAdjunto = c.String(),
                        GrupoUsuarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NoticiaId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Noticia");
        }
    }
}
