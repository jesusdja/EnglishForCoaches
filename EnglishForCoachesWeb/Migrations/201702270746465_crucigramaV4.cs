namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class crucigramaV4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CasillaCrucigrama", "Crucigrama_Id", c => c.Int());
            CreateIndex("dbo.CasillaCrucigrama", "Crucigrama_Id");
            AddForeignKey("dbo.CasillaCrucigrama", "Crucigrama_Id", "dbo.Crucigrama", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CasillaCrucigrama", "Crucigrama_Id", "dbo.Crucigrama");
            DropIndex("dbo.CasillaCrucigrama", new[] { "Crucigrama_Id" });
            DropColumn("dbo.CasillaCrucigrama", "Crucigrama_Id");
        }
    }
}
