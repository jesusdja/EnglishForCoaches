namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fillthegapExplicacionV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FillTheGap", "Explicacion", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FillTheGap", "Explicacion");
        }
    }
}
