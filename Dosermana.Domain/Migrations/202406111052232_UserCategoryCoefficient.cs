namespace Dosermana.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserCategoryCoefficient : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserCategoryCoefficients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        Coefficient = c.Decimal(nullable: false, precision: 5, scale: 3),
                        ProductCategory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategories", t => t.ProductCategory_Id)
                .Index(t => t.ProductCategory_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserCategoryCoefficients", "ProductCategory_Id", "dbo.ProductCategories");
            DropIndex("dbo.UserCategoryCoefficients", new[] { "ProductCategory_Id" });
            DropTable("dbo.UserCategoryCoefficients");
            DropTable("dbo.ProductCategories");
        }
    }
}
