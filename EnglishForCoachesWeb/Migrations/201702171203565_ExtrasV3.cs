namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtrasV3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Extra", "UrlVideo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Extra", "UrlVideo");
        }
    }
}
