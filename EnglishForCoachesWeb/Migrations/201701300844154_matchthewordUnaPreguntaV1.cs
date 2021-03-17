namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class matchthewordUnaPreguntaV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MatchTheWord", "Pregunta", c => c.String(nullable: false));
            AddColumn("dbo.MatchTheWord", "Respuesta", c => c.String(nullable: false));
            DropColumn("dbo.MatchTheWord", "Pregunta1");
            DropColumn("dbo.MatchTheWord", "Pregunta2");
            DropColumn("dbo.MatchTheWord", "Pregunta3");
            DropColumn("dbo.MatchTheWord", "Pregunta4");
            DropColumn("dbo.MatchTheWord", "Pregunta5");
            DropColumn("dbo.MatchTheWord", "Respuesta1");
            DropColumn("dbo.MatchTheWord", "Respuesta2");
            DropColumn("dbo.MatchTheWord", "Respuesta3");
            DropColumn("dbo.MatchTheWord", "Respuesta4");
            DropColumn("dbo.MatchTheWord", "Respuesta5");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MatchTheWord", "Respuesta5", c => c.String());
            AddColumn("dbo.MatchTheWord", "Respuesta4", c => c.String());
            AddColumn("dbo.MatchTheWord", "Respuesta3", c => c.String());
            AddColumn("dbo.MatchTheWord", "Respuesta2", c => c.String(nullable: false));
            AddColumn("dbo.MatchTheWord", "Respuesta1", c => c.String(nullable: false));
            AddColumn("dbo.MatchTheWord", "Pregunta5", c => c.String());
            AddColumn("dbo.MatchTheWord", "Pregunta4", c => c.String());
            AddColumn("dbo.MatchTheWord", "Pregunta3", c => c.String());
            AddColumn("dbo.MatchTheWord", "Pregunta2", c => c.String(nullable: false));
            AddColumn("dbo.MatchTheWord", "Pregunta1", c => c.String(nullable: false));
            DropColumn("dbo.MatchTheWord", "Respuesta");
            DropColumn("dbo.MatchTheWord", "Pregunta");
        }
    }
}
