namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoticiaResumenv1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Noticia", "TextoResumen", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Noticia", "TextoResumen");
        }
    }
}
