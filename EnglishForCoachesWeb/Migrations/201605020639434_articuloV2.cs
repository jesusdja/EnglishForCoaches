namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articuloV2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        TagId = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.TagId);
            
            CreateTable(
                "dbo.TagArticulo",
                c => new
                    {
                        Tag_TagId = c.Int(nullable: false),
                        Articulo_ArticuloId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagId, t.Articulo_ArticuloId })
                .ForeignKey("dbo.Tag", t => t.Tag_TagId, cascadeDelete: true)
                .ForeignKey("dbo.Articulo", t => t.Articulo_ArticuloId, cascadeDelete: true)
                .Index(t => t.Tag_TagId)
                .Index(t => t.Articulo_ArticuloId);
            
            AddColumn("dbo.Articulo", "FechaModificacion", c => c.DateTime(nullable: false));
            AddColumn("dbo.Articulo", "Titulo", c => c.String());
            AddColumn("dbo.Articulo", "ImagenDestacada", c => c.Binary());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TagArticulo", "Articulo_ArticuloId", "dbo.Articulo");
            DropForeignKey("dbo.TagArticulo", "Tag_TagId", "dbo.Tag");
            DropIndex("dbo.TagArticulo", new[] { "Articulo_ArticuloId" });
            DropIndex("dbo.TagArticulo", new[] { "Tag_TagId" });
            DropColumn("dbo.Articulo", "ImagenDestacada");
            DropColumn("dbo.Articulo", "Titulo");
            DropColumn("dbo.Articulo", "FechaModificacion");
            DropTable("dbo.TagArticulo");
            DropTable("dbo.Tag");
        }
    }
}
