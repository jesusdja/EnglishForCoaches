namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtrasOrdenV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Extra", "Orden", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Extra", "Orden");
        }
    }
}
