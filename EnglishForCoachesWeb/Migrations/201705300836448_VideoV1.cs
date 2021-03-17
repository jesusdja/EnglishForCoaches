namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VideoV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Video",
                c => new
                    {
                        VideoId = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false),
                        Descripcion = c.String(nullable: false),
                        FicheroAdjunto = c.String(),
                        Fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.VideoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Video");
        }
    }
}
