namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articulov4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articulo", "FechaPublicacion", c => c.DateTime(nullable: false));
            DropColumn("dbo.Articulo", "FechaModificacion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articulo", "FechaModificacion", c => c.DateTime(nullable: false));
            DropColumn("dbo.Articulo", "FechaPublicacion");
        }
    }
}
