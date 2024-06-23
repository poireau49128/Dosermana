namespace Dosermana.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderItem_new_field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItems", "Order_note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderItems", "Order_note");
        }
    }
}
