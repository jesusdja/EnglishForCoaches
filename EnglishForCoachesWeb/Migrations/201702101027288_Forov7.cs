namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Forov7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ForoHilo", "CreadoPor", c => c.String());
            AddColumn("dbo.ForoMensaje", "CreadoPor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ForoMensaje", "CreadoPor");
            DropColumn("dbo.ForoHilo", "CreadoPor");
        }
    }
}
