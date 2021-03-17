namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EjerciciosV2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Skillwise", "TemaId", "dbo.Tema");
            DropForeignKey("dbo.Test", "TemaId", "dbo.Tema");
            DropIndex("dbo.Skillwise", new[] { "TemaId" });
            DropIndex("dbo.Test", new[] { "TemaId" });
            CreateTable(
                "dbo.TipoEjercicio",
                c => new
                    {
                        TipoEjercicioId = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.TipoEjercicioId);
            
            AddColumn("dbo.Skillwise", "SubTemaId", c => c.Int(nullable: false));
            AddColumn("dbo.Skillwise", "TipoEjercicioId", c => c.Int(nullable: false));
            AddColumn("dbo.Test", "SubTemaId", c => c.Int(nullable: false));
            AddColumn("dbo.Test", "TipoEjercicioId", c => c.Int(nullable: false));
            CreateIndex("dbo.Skillwise", "SubTemaId");
            CreateIndex("dbo.Skillwise", "TipoEjercicioId");
            CreateIndex("dbo.Test", "SubTemaId");
            CreateIndex("dbo.Test", "TipoEjercicioId");
            AddForeignKey("dbo.Skillwise", "SubTemaId", "dbo.SubTema", "SubTemaId", cascadeDelete: true);
            AddForeignKey("dbo.Skillwise", "TipoEjercicioId", "dbo.TipoEjercicio", "TipoEjercicioId", cascadeDelete: true);
            AddForeignKey("dbo.Test", "SubTemaId", "dbo.SubTema", "SubTemaId", cascadeDelete: true);
            AddForeignKey("dbo.Test", "TipoEjercicioId", "dbo.TipoEjercicio", "TipoEjercicioId", cascadeDelete: true);
            DropColumn("dbo.Skillwise", "TemaId");
            DropColumn("dbo.Test", "TemaId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Test", "TemaId", c => c.Int(nullable: false));
            AddColumn("dbo.Skillwise", "TemaId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Test", "TipoEjercicioId", "dbo.TipoEjercicio");
            DropForeignKey("dbo.Test", "SubTemaId", "dbo.SubTema");
            DropForeignKey("dbo.Skillwise", "TipoEjercicioId", "dbo.TipoEjercicio");
            DropForeignKey("dbo.Skillwise", "SubTemaId", "dbo.SubTema");
            DropIndex("dbo.Test", new[] { "TipoEjercicioId" });
            DropIndex("dbo.Test", new[] { "SubTemaId" });
            DropIndex("dbo.Skillwise", new[] { "TipoEjercicioId" });
            DropIndex("dbo.Skillwise", new[] { "SubTemaId" });
            DropColumn("dbo.Test", "TipoEjercicioId");
            DropColumn("dbo.Test", "SubTemaId");
            DropColumn("dbo.Skillwise", "TipoEjercicioId");
            DropColumn("dbo.Skillwise", "SubTemaId");
            DropTable("dbo.TipoEjercicio");
            CreateIndex("dbo.Test", "TemaId");
            CreateIndex("dbo.Skillwise", "TemaId");
            AddForeignKey("dbo.Test", "TemaId", "dbo.Tema", "TemaId", cascadeDelete: true);
            AddForeignKey("dbo.Skillwise", "TemaId", "dbo.Tema", "TemaId", cascadeDelete: true);
        }
    }
}
