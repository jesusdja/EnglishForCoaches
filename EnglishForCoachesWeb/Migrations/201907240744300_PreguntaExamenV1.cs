namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PreguntaExamenV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AudioSentenceExercise", "PreguntaExamen", c => c.Boolean());
            AddColumn("dbo.FillTheGap", "PreguntaExamen", c => c.Boolean());
            AddColumn("dbo.Test", "PreguntaExamen", c => c.Boolean());
            AddColumn("dbo.TrueFalse", "PreguntaExamen", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TrueFalse", "PreguntaExamen");
            DropColumn("dbo.Test", "PreguntaExamen");
            DropColumn("dbo.FillTheGap", "PreguntaExamen");
            DropColumn("dbo.AudioSentenceExercise", "PreguntaExamen");
        }
    }
}
