namespace RatingSystem.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ratingsdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ratings", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ratings", "Date");
        }
    }
}
