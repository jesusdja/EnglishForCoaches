namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FraseVocabularioV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Frase", "VocabularioId", c => c.Int());
            CreateIndex("dbo.Frase", "VocabularioId");
            AddForeignKey("dbo.Frase", "VocabularioId", "dbo.Vocabulario", "VocabularioId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Frase", "VocabularioId", "dbo.Vocabulario");
            DropIndex("dbo.Frase", new[] { "VocabularioId" });
            DropColumn("dbo.Frase", "VocabularioId");
        }
    }
}
