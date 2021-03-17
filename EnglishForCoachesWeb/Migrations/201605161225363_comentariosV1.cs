namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comentariosV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comentario",
                c => new
                    {
                        ComentarioId = c.Int(nullable: false, identity: true),
                        ArticuloId = c.Int(nullable: false),
                        Aceptado = c.Boolean(nullable: false),
                        Texto = c.String(nullable: false),
                        Nombre = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        FechaHoraComentario = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ComentarioId)
                .ForeignKey("dbo.Articulo", t => t.ArticuloId, cascadeDelete: true)
                .Index(t => t.ArticuloId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comentario", "ArticuloId", "dbo.Articulo");
            DropIndex("dbo.Comentario", new[] { "ArticuloId" });
            DropTable("dbo.Comentario");
        }
    }
}
