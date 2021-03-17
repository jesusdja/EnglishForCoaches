namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClienteV5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ForoHilo", "ClienteId", c => c.Int(nullable: false));
            AddColumn("dbo.Noticia", "ClienteId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Noticia", "ClienteId");
            DropColumn("dbo.ForoHilo", "ClienteId");
        }
    }
}
