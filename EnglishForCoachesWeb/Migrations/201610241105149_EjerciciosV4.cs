namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EjerciciosV4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TipoEjercicio", "Controlador", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TipoEjercicio", "Controlador");
        }
    }
}
