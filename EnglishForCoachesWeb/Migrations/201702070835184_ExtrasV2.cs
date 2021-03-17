namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtrasV2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Extra", "Titulo", c => c.String(nullable: false));
            AlterColumn("dbo.Extra", "Explicacion", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Extra", "Explicacion", c => c.String());
            AlterColumn("dbo.Extra", "Titulo", c => c.String());
        }
    }
}
