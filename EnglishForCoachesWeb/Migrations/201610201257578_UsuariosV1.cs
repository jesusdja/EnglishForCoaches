namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuariosV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GrupoUsuario",
                c => new
                    {
                        GrupoUsuarioId = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.GrupoUsuarioId);
            
            AddColumn("dbo.AspNetUsers", "Nombre", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "Apellido1", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "Apellido2", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "GrupoUsuarioId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "GrupoUsuarioId");
            AddForeignKey("dbo.AspNetUsers", "GrupoUsuarioId", "dbo.GrupoUsuario", "GrupoUsuarioId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "GrupoUsuarioId", "dbo.GrupoUsuario");
            DropIndex("dbo.AspNetUsers", new[] { "GrupoUsuarioId" });
            DropColumn("dbo.AspNetUsers", "GrupoUsuarioId");
            DropColumn("dbo.AspNetUsers", "Apellido2");
            DropColumn("dbo.AspNetUsers", "Apellido1");
            DropColumn("dbo.AspNetUsers", "Nombre");
            DropTable("dbo.GrupoUsuario");
        }
    }
}
