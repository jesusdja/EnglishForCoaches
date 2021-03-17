namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ahorcadov4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ahorcado", "Explicacion", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ahorcado", "Explicacion");
        }
    }
}
