namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExplicacionBloqueV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bloque", "Explicacion", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bloque", "Explicacion");
        }
    }
}
