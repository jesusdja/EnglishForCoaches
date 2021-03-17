namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoUsuarioV2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GrupoUsuario", "NumeroMaximoUsuarios", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GrupoUsuario", "NumeroMaximoUsuarios");
        }
    }
}
