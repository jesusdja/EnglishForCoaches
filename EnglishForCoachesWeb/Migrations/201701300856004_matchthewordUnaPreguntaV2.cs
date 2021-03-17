namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class matchthewordUnaPreguntaV2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MatchTheWord", "Explicacion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MatchTheWord", "Explicacion", c => c.String(nullable: false));
        }
    }
}
