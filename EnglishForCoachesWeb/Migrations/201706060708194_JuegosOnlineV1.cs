namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JuegosOnlineV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JuegoOnlineDesbloqueado",
                c => new
                    {
                        JuegoOnlineDesbloqueadoId = c.Int(nullable: false, identity: true),
                        JuegoOnlineId = c.Int(nullable: false),
                        FechaDesbloqueo = c.DateTime(nullable: false),
                        AlumnoId = c.String(),
                    })
                .PrimaryKey(t => t.JuegoOnlineDesbloqueadoId)
                .ForeignKey("dbo.JuegoOnline", t => t.JuegoOnlineId, cascadeDelete: true)
                .Index(t => t.JuegoOnlineId);
            
            CreateTable(
                "dbo.JuegoOnline",
                c => new
                    {
                        JuegoOnlineId = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(nullable: false),
                        Explicacion = c.String(),
                        SubTemaId = c.Int(nullable: false),
                        AreaId = c.Int(nullable: false),
                        TipoJuegoOnlineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JuegoOnlineId)
                .ForeignKey("dbo.Area", t => t.AreaId, cascadeDelete: true)
                .ForeignKey("dbo.SubTema", t => t.SubTemaId, cascadeDelete: true)
                .ForeignKey("dbo.TipoJuegoOnline", t => t.TipoJuegoOnlineId, cascadeDelete: true)
                .Index(t => t.SubTemaId)
                .Index(t => t.AreaId)
                .Index(t => t.TipoJuegoOnlineId);
            
            CreateTable(
                "dbo.TipoJuegoOnline",
                c => new
                    {
                        TipoJuegoOnlineId = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                        Controlador = c.String(),
                        EstiloColor = c.String(),
                    })
                .PrimaryKey(t => t.TipoJuegoOnlineId);
            
            CreateTable(
                "dbo.JuegoOnlineRealizado",
                c => new
                    {
                        JuegoOnlineRealizadoId = c.Int(nullable: false, identity: true),
                        JuegoOnlineId = c.Int(nullable: false),
                        FechaRealizado = c.DateTime(nullable: false),
                        AlumnoId = c.String(),
                    })
                .PrimaryKey(t => t.JuegoOnlineRealizadoId)
                .ForeignKey("dbo.JuegoOnline", t => t.JuegoOnlineId, cascadeDelete: true)
                .Index(t => t.JuegoOnlineId);
            
            AddColumn("dbo.BeatTheGoalie", "JuegoOnline_JuegoOnlineId", c => c.Int());
            CreateIndex("dbo.BeatTheGoalie", "JuegoOnline_JuegoOnlineId");
            AddForeignKey("dbo.BeatTheGoalie", "JuegoOnline_JuegoOnlineId", "dbo.JuegoOnline", "JuegoOnlineId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JuegoOnlineRealizado", "JuegoOnlineId", "dbo.JuegoOnline");
            DropForeignKey("dbo.JuegoOnlineDesbloqueado", "JuegoOnlineId", "dbo.JuegoOnline");
            DropForeignKey("dbo.JuegoOnline", "TipoJuegoOnlineId", "dbo.TipoJuegoOnline");
            DropForeignKey("dbo.JuegoOnline", "SubTemaId", "dbo.SubTema");
            DropForeignKey("dbo.BeatTheGoalie", "JuegoOnline_JuegoOnlineId", "dbo.JuegoOnline");
            DropForeignKey("dbo.JuegoOnline", "AreaId", "dbo.Area");
            DropIndex("dbo.JuegoOnlineRealizado", new[] { "JuegoOnlineId" });
            DropIndex("dbo.JuegoOnline", new[] { "TipoJuegoOnlineId" });
            DropIndex("dbo.JuegoOnline", new[] { "AreaId" });
            DropIndex("dbo.JuegoOnline", new[] { "SubTemaId" });
            DropIndex("dbo.JuegoOnlineDesbloqueado", new[] { "JuegoOnlineId" });
            DropIndex("dbo.BeatTheGoalie", new[] { "JuegoOnline_JuegoOnlineId" });
            DropColumn("dbo.BeatTheGoalie", "JuegoOnline_JuegoOnlineId");
            DropTable("dbo.JuegoOnlineRealizado");
            DropTable("dbo.TipoJuegoOnline");
            DropTable("dbo.JuegoOnline");
            DropTable("dbo.JuegoOnlineDesbloqueado");
        }
    }
}
