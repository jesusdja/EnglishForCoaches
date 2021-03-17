namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoUsuarioV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TipoUsuario",
                c => new
                    {
                        TipoUsuarioId = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.TipoUsuarioId);
            
            AddColumn("dbo.AspNetUsers", "TipoUsuarioId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "TipoUsuarioId");
            AddForeignKey("dbo.AspNetUsers", "TipoUsuarioId", "dbo.TipoUsuario", "TipoUsuarioId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "TipoUsuarioId", "dbo.TipoUsuario");
            DropIndex("dbo.AspNetUsers", new[] { "TipoUsuarioId" });
            DropColumn("dbo.AspNetUsers", "TipoUsuarioId");
            DropTable("dbo.TipoUsuario");
        }
    }
}
