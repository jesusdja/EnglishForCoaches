namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoUsuarioV3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GrupoUsuario", "NumeroMaximoUsuarios", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GrupoUsuario", "NumeroMaximoUsuarios", c => c.String());
        }
    }
}
