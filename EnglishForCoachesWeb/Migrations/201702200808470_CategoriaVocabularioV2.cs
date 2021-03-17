namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoriaVocabularioV2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Vocabulario", "CategoriaVocabularioId");
            AddForeignKey("dbo.Vocabulario", "CategoriaVocabularioId", "dbo.CategoriaVocabulario", "CategoriaVocabularioId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vocabulario", "CategoriaVocabularioId", "dbo.CategoriaVocabulario");
            DropIndex("dbo.Vocabulario", new[] { "CategoriaVocabularioId" });
        }
    }
}
