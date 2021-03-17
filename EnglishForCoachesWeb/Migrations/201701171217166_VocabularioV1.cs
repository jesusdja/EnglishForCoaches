namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VocabularioV1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vocabulario", "Palabra_es", c => c.String(nullable: false));
            AlterColumn("dbo.Vocabulario", "Palabra_en", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vocabulario", "Palabra_en", c => c.String());
            AlterColumn("dbo.Vocabulario", "Palabra_es", c => c.String());
        }
    }
}
