namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AreaAlumnov2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Area", "Orden", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Area", "Orden", c => c.Int(nullable: false));
        }
    }
}
