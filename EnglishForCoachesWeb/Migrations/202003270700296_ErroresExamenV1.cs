namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ErroresExamenV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RespuestaIncorrecta", "Tipo", c => c.String());
            AddColumn("dbo.RespuestaIncorrecta", "PreguntaId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RespuestaIncorrecta", "PreguntaId");
            DropColumn("dbo.RespuestaIncorrecta", "Tipo");
        }
    }
}
