namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JuegosOnlineV2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BeatTheGoalie", "AreaId", "dbo.Area");
            DropForeignKey("dbo.BeatTheGoalie", "BloqueId", "dbo.Bloque");
            DropForeignKey("dbo.BeatTheGoalie", "TipoEjercicioId", "dbo.TipoEjercicio");
            DropForeignKey("dbo.JuegoOnline", "AreaId", "dbo.Area");
            DropForeignKey("dbo.BeatTheGoalie", "JuegoOnline_JuegoOnlineId", "dbo.JuegoOnline");
            DropIndex("dbo.BeatTheGoalie", new[] { "AreaId" });
            DropIndex("dbo.BeatTheGoalie", new[] { "TipoEjercicioId" });
            DropIndex("dbo.BeatTheGoalie", new[] { "BloqueId" });
            DropIndex("dbo.BeatTheGoalie", new[] { "JuegoOnline_JuegoOnlineId" });
            DropIndex("dbo.JuegoOnline", new[] { "AreaId" });
            RenameColumn(table: "dbo.BeatTheGoalie", name: "JuegoOnline_JuegoOnlineId", newName: "JuegoOnlineId");
            AddColumn("dbo.BeatTheGoalie", "TipoJuegoOnlineId", c => c.Int(nullable: false));
            AlterColumn("dbo.BeatTheGoalie", "JuegoOnlineId", c => c.Int(nullable: false));
            CreateIndex("dbo.BeatTheGoalie", "TipoJuegoOnlineId");
            CreateIndex("dbo.BeatTheGoalie", "JuegoOnlineId");
            AddForeignKey("dbo.BeatTheGoalie", "TipoJuegoOnlineId", "dbo.TipoJuegoOnline", "TipoJuegoOnlineId", cascadeDelete: false);
            AddForeignKey("dbo.BeatTheGoalie", "JuegoOnlineId", "dbo.JuegoOnline", "JuegoOnlineId", cascadeDelete: false);
            DropColumn("dbo.BeatTheGoalie", "AreaId");
            DropColumn("dbo.BeatTheGoalie", "TipoEjercicioId");
            DropColumn("dbo.BeatTheGoalie", "BloqueId");
            DropColumn("dbo.JuegoOnline", "AreaId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JuegoOnline", "AreaId", c => c.Int(nullable: false));
            AddColumn("dbo.BeatTheGoalie", "BloqueId", c => c.Int(nullable: false));
            AddColumn("dbo.BeatTheGoalie", "TipoEjercicioId", c => c.Int(nullable: false));
            AddColumn("dbo.BeatTheGoalie", "AreaId", c => c.Int(nullable: false));
            DropForeignKey("dbo.BeatTheGoalie", "JuegoOnlineId", "dbo.JuegoOnline");
            DropForeignKey("dbo.BeatTheGoalie", "TipoJuegoOnlineId", "dbo.TipoJuegoOnline");
            DropIndex("dbo.BeatTheGoalie", new[] { "JuegoOnlineId" });
            DropIndex("dbo.BeatTheGoalie", new[] { "TipoJuegoOnlineId" });
            AlterColumn("dbo.BeatTheGoalie", "JuegoOnlineId", c => c.Int());
            DropColumn("dbo.BeatTheGoalie", "TipoJuegoOnlineId");
            RenameColumn(table: "dbo.BeatTheGoalie", name: "JuegoOnlineId", newName: "JuegoOnline_JuegoOnlineId");
            CreateIndex("dbo.JuegoOnline", "AreaId");
            CreateIndex("dbo.BeatTheGoalie", "JuegoOnline_JuegoOnlineId");
            CreateIndex("dbo.BeatTheGoalie", "BloqueId");
            CreateIndex("dbo.BeatTheGoalie", "TipoEjercicioId");
            CreateIndex("dbo.BeatTheGoalie", "AreaId");
            AddForeignKey("dbo.BeatTheGoalie", "JuegoOnline_JuegoOnlineId", "dbo.JuegoOnline", "JuegoOnlineId");
            AddForeignKey("dbo.JuegoOnline", "AreaId", "dbo.Area", "AreaId", cascadeDelete: true);
            AddForeignKey("dbo.BeatTheGoalie", "TipoEjercicioId", "dbo.TipoEjercicio", "TipoEjercicioId", cascadeDelete: true);
            AddForeignKey("dbo.BeatTheGoalie", "BloqueId", "dbo.Bloque", "BloqueId", cascadeDelete: true);
            AddForeignKey("dbo.BeatTheGoalie", "AreaId", "dbo.Area", "AreaId", cascadeDelete: true);
        }
    }
}
