namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FrasesV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Frase",
                c => new
                    {
                        FraseId = c.Int(nullable: false, identity: true),
                        Palabra_es = c.String(nullable: false),
                        Palabra_en = c.String(nullable: false),
                        FicheroAudio_es = c.String(),
                        FicheroAudio_en = c.String(),
                        GramaticaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FraseId)
                .ForeignKey("dbo.Gramatica", t => t.GramaticaId, cascadeDelete: true)
                .Index(t => t.GramaticaId);
            
            AlterColumn("dbo.Bloque", "Descripcion", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Frase", "GramaticaId", "dbo.Gramatica");
            DropIndex("dbo.Frase", new[] { "GramaticaId" });
            AlterColumn("dbo.Bloque", "Descripcion", c => c.String());
            DropTable("dbo.Frase");
        }
    }
}
