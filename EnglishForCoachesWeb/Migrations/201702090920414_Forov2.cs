namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Forov2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ForoCategoria", "Titulo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ForoCategoria", "Titulo");
        }
    }
}
