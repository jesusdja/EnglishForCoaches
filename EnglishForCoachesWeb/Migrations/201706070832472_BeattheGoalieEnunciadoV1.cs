namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BeattheGoalieEnunciadoV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BeatTheGoalie", "Enunciado", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BeatTheGoalie", "Enunciado");
        }
    }
}
