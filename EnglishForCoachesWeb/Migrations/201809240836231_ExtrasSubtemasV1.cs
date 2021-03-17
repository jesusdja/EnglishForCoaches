namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtrasSubtemasV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Extra", "SubTemaId", c => c.Int());
            CreateIndex("dbo.Extra", "SubTemaId");
            AddForeignKey("dbo.Extra", "SubTemaId", "dbo.SubTema", "SubTemaId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Extra", "SubTemaId", "dbo.SubTema");
            DropIndex("dbo.Extra", new[] { "SubTemaId" });
            DropColumn("dbo.Extra", "SubTemaId");
        }
    }
}
