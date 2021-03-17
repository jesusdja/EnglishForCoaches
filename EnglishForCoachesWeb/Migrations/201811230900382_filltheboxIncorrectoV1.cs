namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class filltheboxIncorrectoV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FillTheBox", "RespuestasIncorrectas", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FillTheBox", "RespuestasIncorrectas");
        }
    }
}
