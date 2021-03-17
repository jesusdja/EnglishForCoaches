namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FraseVocabularioV2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Frase", "VocabularioId", "dbo.Vocabulario");
            DropIndex("dbo.Frase", new[] { "VocabularioId" });
            CreateTable(
                "dbo.VocabularioFrase",
                c => new
                    {
                        Vocabulario_VocabularioId = c.Int(nullable: false),
                        Frase_FraseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Vocabulario_VocabularioId, t.Frase_FraseId })
                .ForeignKey("dbo.Vocabulario", t => t.Vocabulario_VocabularioId, cascadeDelete: true)
                .ForeignKey("dbo.Frase", t => t.Frase_FraseId, cascadeDelete: true)
                .Index(t => t.Vocabulario_VocabularioId)
                .Index(t => t.Frase_FraseId);
            
            DropColumn("dbo.Frase", "VocabularioId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Frase", "VocabularioId", c => c.Int());
            DropForeignKey("dbo.VocabularioFrase", "Frase_FraseId", "dbo.Frase");
            DropForeignKey("dbo.VocabularioFrase", "Vocabulario_VocabularioId", "dbo.Vocabulario");
            DropIndex("dbo.VocabularioFrase", new[] { "Frase_FraseId" });
            DropIndex("dbo.VocabularioFrase", new[] { "Vocabulario_VocabularioId" });
            DropTable("dbo.VocabularioFrase");
            CreateIndex("dbo.Frase", "VocabularioId");
            AddForeignKey("dbo.Frase", "VocabularioId", "dbo.Vocabulario", "VocabularioId");
        }
    }
}
