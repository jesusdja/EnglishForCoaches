namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForoV3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ForoHilo", "Texto", c => c.String(nullable: false));
            AlterColumn("dbo.ForoHilo", "Titulo", c => c.String(nullable: false));
            AlterColumn("dbo.ForoMensaje", "Texto", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ForoMensaje", "Texto", c => c.String());
            AlterColumn("dbo.ForoHilo", "Titulo", c => c.String());
            DropColumn("dbo.ForoHilo", "Texto");
        }
    }
}
