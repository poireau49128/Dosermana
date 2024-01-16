namespace Dosermana.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderItemModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderItems",
                c => new
                {
                    OrderItemId = c.Int(nullable: false, identity: true),
                    ProductId = c.Int(nullable: false),
                    Quantity = c.Int(nullable: false),
                    // Другие поля модели OrderItem
                })
                .PrimaryKey(t => t.OrderItemId);
        }
        
        public override void Down()
        {
        }
    }
}
