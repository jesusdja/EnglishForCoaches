namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VideoV2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Video", "UrlVideo", c => c.String());
            AddColumn("dbo.Video", "ThumbNail", c => c.String());
            DropColumn("dbo.Video", "FicheroAdjunto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Video", "FicheroAdjunto", c => c.String());
            DropColumn("dbo.Video", "ThumbNail");
            DropColumn("dbo.Video", "UrlVideo");
        }
    }
}
