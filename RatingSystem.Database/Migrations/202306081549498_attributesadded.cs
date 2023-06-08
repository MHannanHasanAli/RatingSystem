namespace RatingSystem.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class attributesadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ratings", "Respect", c => c.String());
            AddColumn("dbo.Ratings", "Explanation", c => c.String());
            AddColumn("dbo.Ratings", "Treatment", c => c.String());
            AddColumn("dbo.Ratings", "Overall", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ratings", "Overall");
            DropColumn("dbo.Ratings", "Treatment");
            DropColumn("dbo.Ratings", "Explanation");
            DropColumn("dbo.Ratings", "Respect");
        }
    }
}
