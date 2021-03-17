namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class categoriaVocabularioV3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CategoriaVocabulario", "Descripcion_en", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CategoriaVocabulario", "Descripcion_en");
        }
    }
}
