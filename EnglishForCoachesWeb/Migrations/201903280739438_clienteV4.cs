namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clienteV4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ClienteId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "ClienteId");
            AddForeignKey("dbo.AspNetUsers", "ClienteId", "dbo.Cliente", "ClienteId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "ClienteId", "dbo.Cliente");
            DropIndex("dbo.AspNetUsers", new[] { "ClienteId" });
            DropColumn("dbo.AspNetUsers", "ClienteId");
        }
    }
}
