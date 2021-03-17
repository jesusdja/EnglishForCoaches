namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForoAdminv2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ForoHilo", "Admin", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ForoMensaje", "Admin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ForoMensaje", "Admin", c => c.Boolean());
            AlterColumn("dbo.ForoHilo", "Admin", c => c.Boolean());
        }
    }
}
