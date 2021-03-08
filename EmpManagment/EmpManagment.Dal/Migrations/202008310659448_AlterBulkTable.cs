namespace EmpManagment.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterBulkTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bulks", "Number", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bulks", "Number");
        }
    }
}
