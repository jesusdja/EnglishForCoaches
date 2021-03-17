namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Forov4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ForoHilo", "RespondidoPor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ForoHilo", "RespondidoPor");
        }
    }
}
