namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ejerciciosV7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FillTheNext", "Respuestas", c => c.String(nullable: false));
            AlterColumn("dbo.MatchTheWord", "Pregunta1", c => c.String(nullable: false));
            AlterColumn("dbo.MatchTheWord", "Pregunta2", c => c.String(nullable: false));
            AlterColumn("dbo.MatchTheWord", "Respuesta1", c => c.String(nullable: false));
            AlterColumn("dbo.MatchTheWord", "Respuesta2", c => c.String(nullable: false));
            AlterColumn("dbo.MatchTheWord", "Explicacion", c => c.String(nullable: false));
            AlterColumn("dbo.Skillwise", "Enunciado", c => c.String(nullable: false));
            AlterColumn("dbo.Skillwise", "Respuesta1", c => c.String(nullable: false));
            AlterColumn("dbo.Skillwise", "Respuesta2", c => c.String(nullable: false));
            AlterColumn("dbo.Skillwise", "Explicacion1", c => c.String(nullable: false));
            AlterColumn("dbo.Skillwise", "Explicacion2", c => c.String(nullable: false));
            AlterColumn("dbo.Test", "Enunciado", c => c.String(nullable: false));
            AlterColumn("dbo.Test", "Respuesta1", c => c.String(nullable: false));
            AlterColumn("dbo.Test", "Respuesta2", c => c.String(nullable: false));
            AlterColumn("dbo.Test", "Explicacion", c => c.String(nullable: false));
            AlterColumn("dbo.WordByWord", "Enunciado", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WordByWord", "Enunciado", c => c.String());
            AlterColumn("dbo.Test", "Explicacion", c => c.String());
            AlterColumn("dbo.Test", "Respuesta2", c => c.String());
            AlterColumn("dbo.Test", "Respuesta1", c => c.String());
            AlterColumn("dbo.Test", "Enunciado", c => c.String());
            AlterColumn("dbo.Skillwise", "Explicacion2", c => c.String());
            AlterColumn("dbo.Skillwise", "Explicacion1", c => c.String());
            AlterColumn("dbo.Skillwise", "Respuesta2", c => c.String());
            AlterColumn("dbo.Skillwise", "Respuesta1", c => c.String());
            AlterColumn("dbo.Skillwise", "Enunciado", c => c.String());
            AlterColumn("dbo.MatchTheWord", "Explicacion", c => c.String());
            AlterColumn("dbo.MatchTheWord", "Respuesta2", c => c.String());
            AlterColumn("dbo.MatchTheWord", "Respuesta1", c => c.String());
            AlterColumn("dbo.MatchTheWord", "Pregunta2", c => c.String());
            AlterColumn("dbo.MatchTheWord", "Pregunta1", c => c.String());
            AlterColumn("dbo.FillTheNext", "Respuestas", c => c.String());
        }
    }
}
