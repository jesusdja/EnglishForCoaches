namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BloqueV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bloque",
                c => new
                    {
                        BloqueId = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                        SubTemaId = c.Int(nullable: false),
                        AreaId = c.Int(nullable: false),
                        TipoEjercicioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BloqueId)
                .ForeignKey("dbo.Area", t => t.AreaId, cascadeDelete: false)
                .ForeignKey("dbo.SubTema", t => t.SubTemaId, cascadeDelete: false)
                .ForeignKey("dbo.TipoEjercicio", t => t.TipoEjercicioId, cascadeDelete: false)
                .Index(t => t.SubTemaId)
                .Index(t => t.AreaId)
                .Index(t => t.TipoEjercicioId);
            
            AddColumn("dbo.FillTheNext", "BloqueId", c => c.Int(nullable: false));
            AddColumn("dbo.MatchTheWord", "BloqueId", c => c.Int(nullable: false));
            AddColumn("dbo.Skillwise", "BloqueId", c => c.Int(nullable: false));
            AddColumn("dbo.Test", "BloqueId", c => c.Int(nullable: false));
            AddColumn("dbo.WordByWord", "BloqueId", c => c.Int(nullable: false));
            CreateIndex("dbo.FillTheNext", "BloqueId");
            CreateIndex("dbo.MatchTheWord", "BloqueId");
            CreateIndex("dbo.Skillwise", "BloqueId");
            CreateIndex("dbo.Test", "BloqueId");
            CreateIndex("dbo.WordByWord", "BloqueId");
            AddForeignKey("dbo.FillTheNext", "BloqueId", "dbo.Bloque", "BloqueId", cascadeDelete: true);
            AddForeignKey("dbo.MatchTheWord", "BloqueId", "dbo.Bloque", "BloqueId", cascadeDelete: true);
            AddForeignKey("dbo.Skillwise", "BloqueId", "dbo.Bloque", "BloqueId", cascadeDelete: true);
            AddForeignKey("dbo.Test", "BloqueId", "dbo.Bloque", "BloqueId", cascadeDelete: true);
            AddForeignKey("dbo.WordByWord", "BloqueId", "dbo.Bloque", "BloqueId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WordByWord", "BloqueId", "dbo.Bloque");
            DropForeignKey("dbo.Test", "BloqueId", "dbo.Bloque");
            DropForeignKey("dbo.Skillwise", "BloqueId", "dbo.Bloque");
            DropForeignKey("dbo.MatchTheWord", "BloqueId", "dbo.Bloque");
            DropForeignKey("dbo.FillTheNext", "BloqueId", "dbo.Bloque");
            DropForeignKey("dbo.Bloque", "TipoEjercicioId", "dbo.TipoEjercicio");
            DropForeignKey("dbo.Bloque", "SubTemaId", "dbo.SubTema");
            DropForeignKey("dbo.Bloque", "AreaId", "dbo.Area");
            DropIndex("dbo.WordByWord", new[] { "BloqueId" });
            DropIndex("dbo.Test", new[] { "BloqueId" });
            DropIndex("dbo.Skillwise", new[] { "BloqueId" });
            DropIndex("dbo.MatchTheWord", new[] { "BloqueId" });
            DropIndex("dbo.FillTheNext", new[] { "BloqueId" });
            DropIndex("dbo.Bloque", new[] { "TipoEjercicioId" });
            DropIndex("dbo.Bloque", new[] { "AreaId" });
            DropIndex("dbo.Bloque", new[] { "SubTemaId" });
            DropColumn("dbo.WordByWord", "BloqueId");
            DropColumn("dbo.Test", "BloqueId");
            DropColumn("dbo.Skillwise", "BloqueId");
            DropColumn("dbo.MatchTheWord", "BloqueId");
            DropColumn("dbo.FillTheNext", "BloqueId");
            DropTable("dbo.Bloque");
        }
    }
}
