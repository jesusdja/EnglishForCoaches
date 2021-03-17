namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IconoTema : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tema", "IconoAdmin", c => c.String());
            AddColumn("dbo.Tema", "IconoUsuario", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tema", "IconoUsuario");
            DropColumn("dbo.Tema", "IconoAdmin");
        }
    }
}
