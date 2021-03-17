namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RespuestasExamenV3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Examen", "NombreAlumno", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Examen", "NombreAlumno");
        }
    }
}
