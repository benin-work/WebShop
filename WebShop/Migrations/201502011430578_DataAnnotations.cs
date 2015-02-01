namespace WebShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Client", "Name", c => c.String(maxLength: 50));
            AlterColumn("dbo.Product", "Name", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Product", "Name", c => c.String());
            AlterColumn("dbo.Client", "Name", c => c.String());
        }
    }
}
