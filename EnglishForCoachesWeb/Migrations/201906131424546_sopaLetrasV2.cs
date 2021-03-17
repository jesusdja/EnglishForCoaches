namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sopaLetrasV2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComentarioSopaLetras",
                c => new
                    {
                        ComentarioSopaLetrasId = c.Int(nullable: false, identity: true),
                        Palabra = c.String(),
                        Comentario = c.String(),
                        SopaLetras_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ComentarioSopaLetrasId)
                .ForeignKey("dbo.SopaLetras", t => t.SopaLetras_Id)
                .Index(t => t.SopaLetras_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComentarioSopaLetras", "SopaLetras_Id", "dbo.SopaLetras");
            DropIndex("dbo.ComentarioSopaLetras", new[] { "SopaLetras_Id" });
            DropTable("dbo.ComentarioSopaLetras");
        }
    }
}
