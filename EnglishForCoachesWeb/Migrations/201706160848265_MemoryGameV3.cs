namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MemoryGameV3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MemoryGame", "PalabraImagen", c => c.String(nullable: false, maxLength: 24));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MemoryGame", "PalabraImagen", c => c.String(nullable: false, maxLength: 16));
        }
    }
}
