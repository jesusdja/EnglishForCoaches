namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usuarioNoticiaV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Noticia", "UsuarioId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Noticia", "UsuarioId");
        }
    }
}
