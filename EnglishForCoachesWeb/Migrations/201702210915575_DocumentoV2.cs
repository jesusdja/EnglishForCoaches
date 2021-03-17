namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentoV2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Documento", "SubTemaId", "dbo.SubTema");
            DropIndex("dbo.Documento", new[] { "SubTemaId" });
            AlterColumn("dbo.Documento", "SubTemaId", c => c.Int());
            CreateIndex("dbo.Documento", "SubTemaId");
            AddForeignKey("dbo.Documento", "SubTemaId", "dbo.SubTema", "SubTemaId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documento", "SubTemaId", "dbo.SubTema");
            DropIndex("dbo.Documento", new[] { "SubTemaId" });
            AlterColumn("dbo.Documento", "SubTemaId", c => c.Int(nullable: false));
            CreateIndex("dbo.Documento", "SubTemaId");
            AddForeignKey("dbo.Documento", "SubTemaId", "dbo.SubTema", "SubTemaId", cascadeDelete: true);
        }
    }
}
