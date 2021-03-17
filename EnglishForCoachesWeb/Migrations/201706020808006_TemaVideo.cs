namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TemaVideo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tema", "UrlVideo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tema", "UrlVideo");
        }
    }
}
