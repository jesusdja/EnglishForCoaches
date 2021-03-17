namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class crucigramav2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.CasillaCrucigrama", "CrucigramaId");
            AddForeignKey("dbo.CasillaCrucigrama", "CrucigramaId", "dbo.Crucigrama", "CrucigramaId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CasillaCrucigrama", "CrucigramaId", "dbo.Crucigrama");
            DropIndex("dbo.CasillaCrucigrama", new[] { "CrucigramaId" });
        }
    }
}
