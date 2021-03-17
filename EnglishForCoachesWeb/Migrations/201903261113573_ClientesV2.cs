namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientesV2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cliente", "LogoBlanco", c => c.String());
            AddColumn("dbo.Cliente", "LogoNegro", c => c.String());
            DropColumn("dbo.Cliente", "Logo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cliente", "Logo", c => c.String());
            DropColumn("dbo.Cliente", "LogoNegro");
            DropColumn("dbo.Cliente", "LogoBlanco");
        }
    }
}
