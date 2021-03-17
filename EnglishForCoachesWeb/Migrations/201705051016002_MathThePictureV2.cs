namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MathThePictureV2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MatchThePicture", "UrlImagen", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MatchThePicture", "UrlImagen", c => c.String(nullable: false));
        }
    }
}
