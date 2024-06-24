namespace MVCproject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class next : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoices", "DeliveryCharge", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Invoices", "TotalBill", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invoices", "TotalBill");
            DropColumn("dbo.Invoices", "DeliveryCharge");
        }
    }
}
