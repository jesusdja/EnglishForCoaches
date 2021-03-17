namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gramaticaVocabularioV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VocabularioGramatica",
                c => new
                    {
                        Vocabulario_VocabularioId = c.Int(nullable: false),
                        Gramatica_GramaticaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Vocabulario_VocabularioId, t.Gramatica_GramaticaId })
                .ForeignKey("dbo.Vocabulario", t => t.Vocabulario_VocabularioId, cascadeDelete: true)
                .ForeignKey("dbo.Gramatica", t => t.Gramatica_GramaticaId, cascadeDelete: true)
                .Index(t => t.Vocabulario_VocabularioId)
                .Index(t => t.Gramatica_GramaticaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VocabularioGramatica", "Gramatica_GramaticaId", "dbo.Gramatica");
            DropForeignKey("dbo.VocabularioGramatica", "Vocabulario_VocabularioId", "dbo.Vocabulario");
            DropIndex("dbo.VocabularioGramatica", new[] { "Gramatica_GramaticaId" });
            DropIndex("dbo.VocabularioGramatica", new[] { "Vocabulario_VocabularioId" });
            DropTable("dbo.VocabularioGramatica");
        }
    }
}
