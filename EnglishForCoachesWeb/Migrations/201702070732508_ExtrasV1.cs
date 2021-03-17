namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtrasV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoriaExtra",
                c => new
                    {
                        CategoriaExtraId = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                        Icono = c.String(),
                    })
                .PrimaryKey(t => t.CategoriaExtraId);
            
            CreateTable(
                "dbo.Extra",
                c => new
                    {
                        ExtraId = c.Int(nullable: false, identity: true),
                        Titulo = c.String(),
                        Explicacion = c.String(),
                        Consejo = c.String(),
                        Foto = c.String(),
                        Audio = c.String(),
                        Fichero = c.String(),
                        CategoriaExtraId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExtraId)
                .ForeignKey("dbo.CategoriaExtra", t => t.CategoriaExtraId, cascadeDelete: true)
                .Index(t => t.CategoriaExtraId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Extra", "CategoriaExtraId", "dbo.CategoriaExtra");
            DropIndex("dbo.Extra", new[] { "CategoriaExtraId" });
            DropTable("dbo.Extra");
            DropTable("dbo.CategoriaExtra");
        }
    }
}
