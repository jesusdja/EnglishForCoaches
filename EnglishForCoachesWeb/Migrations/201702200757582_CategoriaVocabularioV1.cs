namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoriaVocabularioV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoriaVocabulario",
                c => new
                    {
                        CategoriaVocabularioId = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.CategoriaVocabularioId);
            
            AddColumn("dbo.Vocabulario", "CategoriaVocabularioId", c => c.Int(nullable: false));
            AddColumn("dbo.Vocabulario", "Explicacion", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vocabulario", "Explicacion");
            DropColumn("dbo.Vocabulario", "CategoriaVocabularioId");
            DropTable("dbo.CategoriaVocabulario");
        }
    }
}
