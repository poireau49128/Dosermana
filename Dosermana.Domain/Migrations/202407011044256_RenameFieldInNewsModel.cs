namespace Dosermana.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameFieldInNewsModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "NewsTitle", c => c.String());
            DropColumn("dbo.News", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.News", "Title", c => c.String());
            DropColumn("dbo.News", "NewsTitle");
        }
    }
}
