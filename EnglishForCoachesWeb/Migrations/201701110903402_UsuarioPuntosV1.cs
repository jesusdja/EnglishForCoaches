namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuarioPuntosV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PuntosTotal", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "PuntosActual", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PuntosActual");
            DropColumn("dbo.AspNetUsers", "PuntosTotal");
        }
    }
}
