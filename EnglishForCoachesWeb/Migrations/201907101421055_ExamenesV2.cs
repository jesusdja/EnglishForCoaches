namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExamenesV2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Examen", "SubTemaId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Examen", "SubTemaId");
        }
    }
}
