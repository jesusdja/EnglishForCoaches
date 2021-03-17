namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class crucigramaV3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CasillaCrucigrama", "CrucigramaId", "dbo.Crucigrama");
            DropForeignKey("dbo.Crucigrama", "CategoriaJuegoId", "dbo.CategoriaJuego");
            DropIndex("dbo.CasillaCrucigrama", new[] { "CrucigramaId" });
            DropIndex("dbo.Crucigrama", new[] { "CategoriaJuegoId" });
            DropPrimaryKey("dbo.Crucigrama");
            DropColumn("dbo.CasillaCrucigrama", "CrucigramaId");
            DropColumn("dbo.Crucigrama", "CrucigramaId");
            AddColumn("dbo.Crucigrama", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Crucigrama", "Enunciado", c => c.String(nullable: false));
            AddColumn("dbo.Crucigrama", "Descripcion", c => c.String());
            AddColumn("dbo.Crucigrama", "AreaId", c => c.Int(nullable: false));
            AddColumn("dbo.Crucigrama", "TipoEjercicioId", c => c.Int(nullable: false));
            AddColumn("dbo.Crucigrama", "BloqueId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Crucigrama", "Id");
            CreateIndex("dbo.Crucigrama", "AreaId");
            CreateIndex("dbo.Crucigrama", "TipoEjercicioId");
            CreateIndex("dbo.Crucigrama", "BloqueId");
            AddForeignKey("dbo.Crucigrama", "AreaId", "dbo.Area", "AreaId", cascadeDelete: true);
            AddForeignKey("dbo.Crucigrama", "BloqueId", "dbo.Bloque", "BloqueId", cascadeDelete: true);
            AddForeignKey("dbo.Crucigrama", "TipoEjercicioId", "dbo.TipoEjercicio", "TipoEjercicioId", cascadeDelete: true);
          
            DropColumn("dbo.Crucigrama", "Titulo");
            DropColumn("dbo.Crucigrama", "CategoriaJuegoId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Crucigrama", "CategoriaJuegoId", c => c.Int(nullable: false));
            AddColumn("dbo.Crucigrama", "Titulo", c => c.String(nullable: false));
            AddColumn("dbo.Crucigrama", "CrucigramaId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.CasillaCrucigrama", "CrucigramaId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Crucigrama", "TipoEjercicioId", "dbo.TipoEjercicio");
            DropForeignKey("dbo.Crucigrama", "BloqueId", "dbo.Bloque");
            DropForeignKey("dbo.Crucigrama", "AreaId", "dbo.Area");
            DropIndex("dbo.Crucigrama", new[] { "BloqueId" });
            DropIndex("dbo.Crucigrama", new[] { "TipoEjercicioId" });
            DropIndex("dbo.Crucigrama", new[] { "AreaId" });
            DropPrimaryKey("dbo.Crucigrama");
            DropColumn("dbo.Crucigrama", "BloqueId");
            DropColumn("dbo.Crucigrama", "TipoEjercicioId");
            DropColumn("dbo.Crucigrama", "AreaId");
            DropColumn("dbo.Crucigrama", "Descripcion");
            DropColumn("dbo.Crucigrama", "Enunciado");
            DropColumn("dbo.Crucigrama", "Id");
            AddPrimaryKey("dbo.Crucigrama", "CrucigramaId");
            CreateIndex("dbo.Crucigrama", "CategoriaJuegoId");
            CreateIndex("dbo.CasillaCrucigrama", "CrucigramaId");
            AddForeignKey("dbo.Crucigrama", "CategoriaJuegoId", "dbo.CategoriaJuego", "CategoriaJuegoId", cascadeDelete: true);
            AddForeignKey("dbo.CasillaCrucigrama", "CrucigramaId", "dbo.Crucigrama", "CrucigramaId", cascadeDelete: true);
        }
    }
}
