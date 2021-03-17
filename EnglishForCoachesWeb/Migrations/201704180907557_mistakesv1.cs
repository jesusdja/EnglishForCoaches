namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mistakesv1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Mistake",
                c => new
                    {
                        MistakeId = c.Int(nullable: false, identity: true),
                        AlumnoId = c.String(),
                        BloqueId = c.Int(nullable: false),
                        PreguntaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MistakeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Mistake");
        }
    }
}
