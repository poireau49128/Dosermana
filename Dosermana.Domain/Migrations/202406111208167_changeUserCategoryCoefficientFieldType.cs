namespace Dosermana.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeUserCategoryCoefficientFieldType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserCategoryCoefficients", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserCategoryCoefficients", "UserId", c => c.Int(nullable: false));
        }
    }
}
