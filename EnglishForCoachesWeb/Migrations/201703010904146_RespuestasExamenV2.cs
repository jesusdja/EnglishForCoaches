namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RespuestasExamenV2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RespuestaIncorrecta", "RespuestaSeleccionada", c => c.String());
            AddColumn("dbo.RespuestaIncorrecta", "RespuestaCorrecta", c => c.String());
            DropColumn("dbo.RespuestaIncorrecta", "Respuesta");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RespuestaIncorrecta", "Respuesta", c => c.String());
            DropColumn("dbo.RespuestaIncorrecta", "RespuestaCorrecta");
            DropColumn("dbo.RespuestaIncorrecta", "RespuestaSeleccionada");
        }
    }
}
