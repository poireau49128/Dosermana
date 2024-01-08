namespace Dosermana.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PendingChanges : DbMigration
    {
        public override void Down()
        {
            //DropColumn("dbo.AspNetUsers", "Price_coefficient");
        }
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Price_coefficient", c => c.Decimal(nullable: false, precision: 5, scale: 3, defaultValue: 1));
        }
        
        
    }
}
