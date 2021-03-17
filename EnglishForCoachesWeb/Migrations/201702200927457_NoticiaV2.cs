namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoticiaV2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NoticiaGrupo",
                c => new
                    {
                        NoticiaGrupoId = c.Int(nullable: false, identity: true),
                        NoticiaId = c.Int(nullable: false),
                        GrupoUsuarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NoticiaGrupoId)
                .ForeignKey("dbo.GrupoUsuario", t => t.GrupoUsuarioId, cascadeDelete: true)
                .Index(t => t.GrupoUsuarioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NoticiaGrupo", "GrupoUsuarioId", "dbo.GrupoUsuario");
            DropIndex("dbo.NoticiaGrupo", new[] { "GrupoUsuarioId" });
            DropTable("dbo.NoticiaGrupo");
        }
    }
}
