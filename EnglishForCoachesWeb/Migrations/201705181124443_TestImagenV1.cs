namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestImagenV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Test", "UrlImagen", c => c.String());
            AlterColumn("dbo.Test", "Enunciado", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Test", "Enunciado", c => c.String(nullable: false));
            DropColumn("dbo.Test", "UrlImagen");
        }
    }
}
