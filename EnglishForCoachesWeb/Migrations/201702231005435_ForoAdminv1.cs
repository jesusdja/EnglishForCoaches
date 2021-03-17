namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForoAdminv1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ForoHilo", "Admin", c => c.Boolean());
            AddColumn("dbo.ForoMensaje", "Admin", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ForoMensaje", "Admin");
            DropColumn("dbo.ForoHilo", "Admin");
        }
    }
}
