namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trueFalseAudioV1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrueFalse", "Audio", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TrueFalse", "Audio");
        }
    }
}
