namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoticiaV4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Noticia", "Fecha", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Noticia", "Fecha");
        }
    }
}
