namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoEjercicioAlumnoV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TipoEjercicio", "EstiloColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TipoEjercicio", "EstiloColor");
        }
    }
}
