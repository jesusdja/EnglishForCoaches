namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubtemaDesbloqueadoV2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubTemaDesbloqueado", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropIndex("dbo.SubTemaDesbloqueado", new[] { "ApplicationUser_Id1" });
            AlterColumn("dbo.SubTemaDesbloqueado", "ApplicationUser_Id", c => c.String());
            DropColumn("dbo.SubTemaDesbloqueado", "ApplicationUser_Id1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubTemaDesbloqueado", "ApplicationUser_Id1", c => c.String(maxLength: 128));
            AlterColumn("dbo.SubTemaDesbloqueado", "ApplicationUser_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.SubTemaDesbloqueado", "ApplicationUser_Id1");
            AddForeignKey("dbo.SubTemaDesbloqueado", "ApplicationUser_Id1", "dbo.AspNetUsers", "Id");
        }
    }
}
