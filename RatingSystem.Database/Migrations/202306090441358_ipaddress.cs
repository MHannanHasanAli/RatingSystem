namespace RatingSystem.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ipaddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ratings", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ratings", "Address");
        }
    }
}
