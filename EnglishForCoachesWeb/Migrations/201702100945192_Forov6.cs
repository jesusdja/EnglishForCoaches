namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Forov6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ForoHilo", "AlumnoRespuestaId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ForoHilo", "AlumnoRespuestaId");
        }
    }
}
