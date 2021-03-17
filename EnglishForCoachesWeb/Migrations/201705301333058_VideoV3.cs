namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VideoV3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Video", "Descripcion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Video", "Descripcion", c => c.String(nullable: false));
        }
    }
}
