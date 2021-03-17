namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentoV3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documento", "TemaId", c => c.Int());
            CreateIndex("dbo.Documento", "TemaId");
            AddForeignKey("dbo.Documento", "TemaId", "dbo.Tema", "TemaId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documento", "TemaId", "dbo.Tema");
            DropIndex("dbo.Documento", new[] { "TemaId" });
            DropColumn("dbo.Documento", "TemaId");
        }
    }
}
