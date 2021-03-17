namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vocabularioGlosarioV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VocabularioGlosario",
                c => new
                    {
                        VocabularioGlosarioId = c.Int(nullable: false, identity: true),
                        VocabularioId = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        AlumnoId = c.String(),
                    })
                .PrimaryKey(t => t.VocabularioGlosarioId)
                .ForeignKey("dbo.Vocabulario", t => t.VocabularioId, cascadeDelete: true)
                .Index(t => t.VocabularioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VocabularioGlosario", "VocabularioId", "dbo.Vocabulario");
            DropIndex("dbo.VocabularioGlosario", new[] { "VocabularioId" });
            DropTable("dbo.VocabularioGlosario");
        }
    }
}
