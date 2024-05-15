namespace Dosermana.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Description", c => c.String());
            DropColumn("dbo.OrderItems", "Order_note");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderItems", "Order_note", c => c.String());
            DropColumn("dbo.Products", "Description");
        }
    }
}
