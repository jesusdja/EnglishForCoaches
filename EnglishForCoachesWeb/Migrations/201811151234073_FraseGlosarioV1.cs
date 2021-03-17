namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FraseGlosarioV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FraseGlosario",
                c => new
                    {
                        FraseGlosarioId = c.Int(nullable: false, identity: true),
                        FraseId = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        AlumnoId = c.String(),
                    })
                .PrimaryKey(t => t.FraseGlosarioId)
                .ForeignKey("dbo.Frase", t => t.FraseId, cascadeDelete: true)
                .Index(t => t.FraseId);
            
            AddColumn("dbo.Frase", "Comentario", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FraseGlosario", "FraseId", "dbo.Frase");
            DropIndex("dbo.FraseGlosario", new[] { "FraseId" });
            DropColumn("dbo.Frase", "Comentario");
            DropTable("dbo.FraseGlosario");
        }
    }
}
