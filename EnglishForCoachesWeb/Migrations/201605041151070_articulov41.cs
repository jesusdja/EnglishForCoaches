namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articulov41 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articulo", "TituloSeo", c => c.String());
            AddColumn("dbo.Articulo", "CuerpoResumen", c => c.String());
            AlterColumn("dbo.Articulo", "Titulo", c => c.String(nullable: false));
            AlterColumn("dbo.Articulo", "Cuerpo", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Articulo", "Cuerpo", c => c.String());
            AlterColumn("dbo.Articulo", "Titulo", c => c.String());
            DropColumn("dbo.Articulo", "CuerpoResumen");
            DropColumn("dbo.Articulo", "TituloSeo");
        }
    }
}
