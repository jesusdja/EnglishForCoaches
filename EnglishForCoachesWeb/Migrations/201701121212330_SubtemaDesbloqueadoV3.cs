namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubtemaDesbloqueadoV3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SubTemaDesbloqueado", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.SubTemaDesbloqueado", "ApplicationUser_Id");
            AddForeignKey("dbo.SubTemaDesbloqueado", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubTemaDesbloqueado", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.SubTemaDesbloqueado", new[] { "ApplicationUser_Id" });
            AlterColumn("dbo.SubTemaDesbloqueado", "ApplicationUser_Id", c => c.String());
        }
    }
}
