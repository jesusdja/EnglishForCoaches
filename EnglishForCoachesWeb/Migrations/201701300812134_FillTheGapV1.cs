namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FillTheGapV1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.FillTheNext", newName: "FillTheGap");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.FillTheGap", newName: "FillTheNext");
        }
    }
}
