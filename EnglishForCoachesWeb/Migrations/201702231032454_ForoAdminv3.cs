namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForoAdminv3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ForoHilo", "RespondidoPorAdmin", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ForoHilo", "RespondidoPorAdmin");
        }
    }
}
