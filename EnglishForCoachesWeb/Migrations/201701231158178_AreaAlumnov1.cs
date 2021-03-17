namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AreaAlumnov1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Area", "Orden", c => c.Int(nullable: false));
            AddColumn("dbo.Area", "Icono", c => c.String());
            AddColumn("dbo.Area", "EstiloColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Area", "EstiloColor");
            DropColumn("dbo.Area", "Icono");
            DropColumn("dbo.Area", "Orden");
        }
    }
}
