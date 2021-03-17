namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SepararHtmlV2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Gramatica", "Cuerpo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Gramatica", "Cuerpo", c => c.String(nullable: false));
        }
    }
}
