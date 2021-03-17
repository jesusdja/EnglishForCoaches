namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class definicionVocabulariov1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DefinicionVocabulario",
                c => new
                    {
                        DefinicionVocabularioId = c.Int(nullable: false, identity: true),
                        VocabularioId = c.Int(nullable: false),
                        Palabra_es = c.String(nullable: false),
                        Definicion = c.String(),
                    })
                .PrimaryKey(t => t.DefinicionVocabularioId)
                .ForeignKey("dbo.Vocabulario", t => t.VocabularioId, cascadeDelete: true)
                .Index(t => t.VocabularioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DefinicionVocabulario", "VocabularioId", "dbo.Vocabulario");
            DropIndex("dbo.DefinicionVocabulario", new[] { "VocabularioId" });
            DropTable("dbo.DefinicionVocabulario");
        }
    }
}
