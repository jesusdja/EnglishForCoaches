namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeoV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categoria", "DescripcionSeo", c => c.String());
            AddColumn("dbo.Tag", "DescripcionSeo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tag", "DescripcionSeo");
            DropColumn("dbo.Categoria", "DescripcionSeo");
        }
    }
}
