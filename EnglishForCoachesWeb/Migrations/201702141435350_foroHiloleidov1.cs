namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foroHiloleidov1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ForoHiloLeido",
                c => new
                    {
                        ForoHiloLeidoId = c.Int(nullable: false, identity: true),
                        ForoHiloId = c.Int(nullable: false),
                        AlumnoId = c.String(),
                    })
                .PrimaryKey(t => t.ForoHiloLeidoId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ForoHiloLeido");
        }
    }
}
