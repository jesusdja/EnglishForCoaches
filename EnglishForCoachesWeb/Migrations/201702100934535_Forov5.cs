namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Forov5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ForoHilo", "FechaRespuesta", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ForoHilo", "FechaRespuesta");
        }
    }
}
