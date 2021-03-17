namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GramaticaV2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gramatica", "Cuerpo", c => c.String(nullable: false));
            AlterColumn("dbo.Gramatica", "Titulo", c => c.String(nullable: false));
            DropColumn("dbo.Gramatica", "Descripcion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Gramatica", "Descripcion", c => c.String());
            AlterColumn("dbo.Gramatica", "Titulo", c => c.String());
            DropColumn("dbo.Gramatica", "Cuerpo");
        }
    }
}
