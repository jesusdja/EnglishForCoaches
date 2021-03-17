namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubtemaDesbloqueadoV4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubTemaDesbloqueado", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.SubTemaDesbloqueado", new[] { "ApplicationUser_Id" });
            AddColumn("dbo.SubTemaDesbloqueado", "AlumnoId", c => c.String());
            DropColumn("dbo.SubTemaDesbloqueado", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubTemaDesbloqueado", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.SubTemaDesbloqueado", "AlumnoId");
            CreateIndex("dbo.SubTemaDesbloqueado", "ApplicationUser_Id");
            AddForeignKey("dbo.SubTemaDesbloqueado", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
