namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForoAdminv4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ForoHilo", "RespondidoPorAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ForoHilo", "RespondidoPorAdmin", c => c.Boolean());
        }
    }
}
