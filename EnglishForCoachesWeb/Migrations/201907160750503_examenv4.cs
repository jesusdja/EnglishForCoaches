namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class examenv4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Examen", "BloqueId", "dbo.Bloque");
            DropIndex("dbo.Examen", new[] { "BloqueId" });
            CreateIndex("dbo.Examen", "SubTemaId");
            AddForeignKey("dbo.Examen", "SubTemaId", "dbo.SubTema", "SubTemaId", cascadeDelete: false);
            DropColumn("dbo.Examen", "BloqueId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Examen", "BloqueId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Examen", "SubTemaId", "dbo.SubTema");
            DropIndex("dbo.Examen", new[] { "SubTemaId" });
            CreateIndex("dbo.Examen", "BloqueId");
            AddForeignKey("dbo.Examen", "BloqueId", "dbo.Bloque", "BloqueId", cascadeDelete: true);
        }
    }
}
