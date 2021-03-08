namespace EmpManagment.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterBulkTableAgain : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Bulks", "Number");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bulks", "Number", c => c.Int(nullable: false));
        }
    }
}
