namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articulov3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articulo", "ImagenDestacada1200", c => c.Binary());
            AddColumn("dbo.Articulo", "ImagenDestacada800", c => c.Binary());
            AddColumn("dbo.Articulo", "ImagenDestacada370", c => c.Binary());
            AddColumn("dbo.Articulo", "ImagenDestacada140", c => c.Binary());
            DropColumn("dbo.Articulo", "ImagenDestacada");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articulo", "ImagenDestacada", c => c.Binary());
            DropColumn("dbo.Articulo", "ImagenDestacada140");
            DropColumn("dbo.Articulo", "ImagenDestacada370");
            DropColumn("dbo.Articulo", "ImagenDestacada800");
            DropColumn("dbo.Articulo", "ImagenDestacada1200");
        }
    }
}
