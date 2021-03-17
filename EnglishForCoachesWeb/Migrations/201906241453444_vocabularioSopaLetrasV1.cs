namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vocabularioSopaLetrasV1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.VocabularioFrase", newName: "FraseVocabulario");
            RenameTable(name: "dbo.VocabularioGramatica", newName: "GramaticaVocabulario");
            DropPrimaryKey("dbo.FraseVocabulario");
            DropPrimaryKey("dbo.GramaticaVocabulario");
            CreateTable(
                "dbo.VocabularioSopaLetras",
                c => new
                    {
                        VocabularioSopaLetrasId = c.Int(nullable: false, identity: true),
                        VocabularioId = c.Int(nullable: false),
                        Comentario = c.String(),
                        SopaLetras_Id = c.Int(),
                    })
                .PrimaryKey(t => t.VocabularioSopaLetrasId)
                .ForeignKey("dbo.SopaLetras", t => t.SopaLetras_Id)
                .ForeignKey("dbo.Vocabulario", t => t.VocabularioId, cascadeDelete: false)
                .Index(t => t.VocabularioId)
                .Index(t => t.SopaLetras_Id);
            
            AddPrimaryKey("dbo.FraseVocabulario", new[] { "Frase_FraseId", "Vocabulario_VocabularioId" });
            AddPrimaryKey("dbo.GramaticaVocabulario", new[] { "Gramatica_GramaticaId", "Vocabulario_VocabularioId" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VocabularioSopaLetras", "VocabularioId", "dbo.Vocabulario");
            DropForeignKey("dbo.VocabularioSopaLetras", "SopaLetras_Id", "dbo.SopaLetras");
            DropIndex("dbo.VocabularioSopaLetras", new[] { "SopaLetras_Id" });
            DropIndex("dbo.VocabularioSopaLetras", new[] { "VocabularioId" });
            DropPrimaryKey("dbo.GramaticaVocabulario");
            DropPrimaryKey("dbo.FraseVocabulario");
            DropTable("dbo.VocabularioSopaLetras");
            AddPrimaryKey("dbo.GramaticaVocabulario", new[] { "Vocabulario_VocabularioId", "Gramatica_GramaticaId" });
            AddPrimaryKey("dbo.FraseVocabulario", new[] { "Vocabulario_VocabularioId", "Frase_FraseId" });
            RenameTable(name: "dbo.GramaticaVocabulario", newName: "VocabularioGramatica");
            RenameTable(name: "dbo.FraseVocabulario", newName: "VocabularioFrase");
        }
    }
}
