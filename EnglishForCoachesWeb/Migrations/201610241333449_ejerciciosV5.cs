namespace EnglishForCoachesWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ejerciciosV5 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.MatchTheWord");
            DropPrimaryKey("dbo.Skillwise");
            DropPrimaryKey("dbo.Test");
            DropColumn("dbo.MatchTheWord", "MatchTheWordId");
            DropColumn("dbo.Skillwise", "SkillwiseId");
            DropColumn("dbo.Test", "TestId");
            AddColumn("dbo.MatchTheWord", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Skillwise", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Test", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.MatchTheWord", "Id");
            AddPrimaryKey("dbo.Skillwise", "Id");
            AddPrimaryKey("dbo.Test", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Test", "TestId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.Skillwise", "SkillwiseId", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.MatchTheWord", "MatchTheWordId", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Test");
            DropPrimaryKey("dbo.Skillwise");
            DropPrimaryKey("dbo.MatchTheWord");
            DropColumn("dbo.Test", "Id");
            DropColumn("dbo.Skillwise", "Id");
            DropColumn("dbo.MatchTheWord", "Id");
            AddPrimaryKey("dbo.Test", "TestId");
            AddPrimaryKey("dbo.Skillwise", "SkillwiseId");
            AddPrimaryKey("dbo.MatchTheWord", "MatchTheWordId");
        }
    }
}
