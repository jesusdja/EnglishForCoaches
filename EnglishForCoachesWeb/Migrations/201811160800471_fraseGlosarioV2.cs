namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fraseGlosarioV2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Frase", "Comentario", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Frase", "Comentario", c => c.String(nullable: false));
        }
    }
}
