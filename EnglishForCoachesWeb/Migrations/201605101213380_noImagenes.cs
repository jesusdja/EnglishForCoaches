namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class noImagenes : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Articulo", "ImagenDestacada1200");
            DropColumn("dbo.Articulo", "ImagenDestacada800");
            DropColumn("dbo.Articulo", "ImagenDestacada370");
            DropColumn("dbo.Articulo", "ImagenDestacada140");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articulo", "ImagenDestacada140", c => c.Binary());
            AddColumn("dbo.Articulo", "ImagenDestacada370", c => c.Binary());
            AddColumn("dbo.Articulo", "ImagenDestacada800", c => c.Binary());
            AddColumn("dbo.Articulo", "ImagenDestacada1200", c => c.Binary());
        }
    }
}
