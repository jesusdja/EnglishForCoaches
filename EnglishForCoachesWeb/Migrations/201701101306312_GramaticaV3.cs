namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GramaticaV3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Gramatica", "AreaId", "dbo.Area");
            DropIndex("dbo.Gramatica", new[] { "AreaId" });
            DropColumn("dbo.Gramatica", "AreaId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Gramatica", "AreaId", c => c.Int(nullable: false));
            CreateIndex("dbo.Gramatica", "AreaId");
            AddForeignKey("dbo.Gramatica", "AreaId", "dbo.Area", "AreaId", cascadeDelete: true);
        }
    }
}
