namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccesoUsuarioSubteV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "BloquearSubtemas", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "BloquearSubtemas");
        }
    }
}
