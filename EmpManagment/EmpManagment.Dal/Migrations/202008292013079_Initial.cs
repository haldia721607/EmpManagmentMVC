namespace EmpManagment.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BikeCategories",
                c => new
                    {
                        BikeCategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Description = c.String(nullable: false, maxLength: 500),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BikeCategoryId);
            
            CreateTable(
                "dbo.BikeCollections",
                c => new
                    {
                        BikeCollectionId = c.Int(nullable: false, identity: true),
                        ComplaientDetailsId = c.Int(nullable: false),
                        BikeCategoryId = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BikeCollectionId)
                .ForeignKey("dbo.BikeCategories", t => t.BikeCategoryId)
                .ForeignKey("dbo.ComplaientDetails", t => t.ComplaientDetailsId)
                .Index(t => t.ComplaientDetailsId)
                .Index(t => t.BikeCategoryId);
            
            CreateTable(
                "dbo.ComplaientDetails",
                c => new
                    {
                        ComplaientDetailsId = c.Int(nullable: false, identity: true),
                        ComplaientId = c.Int(nullable: false),
                        ComplaientCategoryId = c.Int(nullable: false),
                        GenderId = c.Int(nullable: false),
                        CountryId = c.Int(nullable: false),
                        StateId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 500),
                        ComplaientDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ComplaientDetailsId)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Complaients", t => t.ComplaientId)
                .ForeignKey("dbo.ComplaientCategories", t => t.ComplaientCategoryId)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .ForeignKey("dbo.Genders", t => t.GenderId)
                .ForeignKey("dbo.States", t => t.StateId)
                .Index(t => t.ComplaientId)
                .Index(t => t.ComplaientCategoryId)
                .Index(t => t.GenderId)
                .Index(t => t.CountryId)
                .Index(t => t.StateId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityId = c.Int(nullable: false, identity: true),
                        StateId = c.Int(nullable: false),
                        CityName = c.String(nullable: false, maxLength: 200),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CityId)
                .ForeignKey("dbo.States", t => t.StateId)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        StateId = c.Int(nullable: false, identity: true),
                        CountryId = c.Int(nullable: false),
                        StateName = c.String(nullable: false, maxLength: 200),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StateId)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        CountryName = c.String(nullable: false, maxLength: 200),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "dbo.Complaients",
                c => new
                    {
                        ComplaientId = c.Int(nullable: false, identity: true),
                        ComplainantName = c.String(nullable: false, maxLength: 200),
                        ComplainantEmail = c.String(nullable: false, maxLength: 200),
                        ComplaientStatus = c.Boolean(nullable: false),
                        CompaientDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ComplaientId);
            
            CreateTable(
                "dbo.ComplaientCategories",
                c => new
                    {
                        ComplaientCategoryId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 500),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ComplaientCategoryId);
            
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        GenderId = c.Int(nullable: false, identity: true),
                        GenderName = c.String(nullable: false, maxLength: 200),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GenderId);
            
            CreateTable(
                "dbo.Bulks",
                c => new
                    {
                        BulkId = c.Int(nullable: false, identity: true),
                        ComplaientDetailsId = c.Int(nullable: false),
                        FileStoreMode = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifyDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BulkId)
                .ForeignKey("dbo.ComplaientDetails", t => t.ComplaientDetailsId)
                .Index(t => t.ComplaientDetailsId);
            
            CreateTable(
                "dbo.BulkDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BulkId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Des = c.String(nullable: false),
                        Email = c.String(),
                        MobileNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bulks", t => t.BulkId)
                .Index(t => t.BulkId);
            
            CreateTable(
                "dbo.ComplaientPermamentAddresses",
                c => new
                    {
                        ComplaientPermamentAddressId = c.Int(nullable: false, identity: true),
                        ComplaientDetailsId = c.Int(nullable: false),
                        Address = c.String(nullable: false, maxLength: 500),
                        PostalCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ComplaientPermamentAddressId)
                .ForeignKey("dbo.ComplaientDetails", t => t.ComplaientDetailsId)
                .Index(t => t.ComplaientDetailsId);
            
            CreateTable(
                "dbo.ComplaientTempAddresses",
                c => new
                    {
                        ComplaientTempAddressId = c.Int(nullable: false, identity: true),
                        ComplaientDetailsId = c.Int(nullable: false),
                        Address = c.String(nullable: false, maxLength: 500),
                        PostalCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ComplaientTempAddressId)
                .ForeignKey("dbo.ComplaientDetails", t => t.ComplaientDetailsId)
                .Index(t => t.ComplaientDetailsId);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ComplaientDetailsId = c.Int(nullable: false),
                        Name = c.String(),
                        ContentType = c.String(),
                        FileEncodingTypes = c.String(),
                        FileStoreMode = c.String(),
                        Path = c.String(),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ComplaientDetails", t => t.ComplaientDetailsId)
                .Index(t => t.ComplaientDetailsId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CountryId = c.Int(nullable: false),
                        StateId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .ForeignKey("dbo.States", t => t.StateId)
                .Index(t => t.CountryId)
                .Index(t => t.StateId)
                .Index(t => t.CityId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "StateId", "dbo.States");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "CityId", "dbo.Cities");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Files", "ComplaientDetailsId", "dbo.ComplaientDetails");
            DropForeignKey("dbo.ComplaientTempAddresses", "ComplaientDetailsId", "dbo.ComplaientDetails");
            DropForeignKey("dbo.ComplaientPermamentAddresses", "ComplaientDetailsId", "dbo.ComplaientDetails");
            DropForeignKey("dbo.BulkDatas", "BulkId", "dbo.Bulks");
            DropForeignKey("dbo.Bulks", "ComplaientDetailsId", "dbo.ComplaientDetails");
            DropForeignKey("dbo.BikeCollections", "ComplaientDetailsId", "dbo.ComplaientDetails");
            DropForeignKey("dbo.ComplaientDetails", "StateId", "dbo.States");
            DropForeignKey("dbo.ComplaientDetails", "GenderId", "dbo.Genders");
            DropForeignKey("dbo.ComplaientDetails", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.ComplaientDetails", "ComplaientCategoryId", "dbo.ComplaientCategories");
            DropForeignKey("dbo.ComplaientDetails", "ComplaientId", "dbo.Complaients");
            DropForeignKey("dbo.ComplaientDetails", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Cities", "StateId", "dbo.States");
            DropForeignKey("dbo.States", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.BikeCollections", "BikeCategoryId", "dbo.BikeCategories");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "CityId" });
            DropIndex("dbo.AspNetUsers", new[] { "StateId" });
            DropIndex("dbo.AspNetUsers", new[] { "CountryId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Files", new[] { "ComplaientDetailsId" });
            DropIndex("dbo.ComplaientTempAddresses", new[] { "ComplaientDetailsId" });
            DropIndex("dbo.ComplaientPermamentAddresses", new[] { "ComplaientDetailsId" });
            DropIndex("dbo.BulkDatas", new[] { "BulkId" });
            DropIndex("dbo.Bulks", new[] { "ComplaientDetailsId" });
            DropIndex("dbo.States", new[] { "CountryId" });
            DropIndex("dbo.Cities", new[] { "StateId" });
            DropIndex("dbo.ComplaientDetails", new[] { "CityId" });
            DropIndex("dbo.ComplaientDetails", new[] { "StateId" });
            DropIndex("dbo.ComplaientDetails", new[] { "CountryId" });
            DropIndex("dbo.ComplaientDetails", new[] { "GenderId" });
            DropIndex("dbo.ComplaientDetails", new[] { "ComplaientCategoryId" });
            DropIndex("dbo.ComplaientDetails", new[] { "ComplaientId" });
            DropIndex("dbo.BikeCollections", new[] { "BikeCategoryId" });
            DropIndex("dbo.BikeCollections", new[] { "ComplaientDetailsId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Files");
            DropTable("dbo.ComplaientTempAddresses");
            DropTable("dbo.ComplaientPermamentAddresses");
            DropTable("dbo.BulkDatas");
            DropTable("dbo.Bulks");
            DropTable("dbo.Genders");
            DropTable("dbo.ComplaientCategories");
            DropTable("dbo.Complaients");
            DropTable("dbo.Countries");
            DropTable("dbo.States");
            DropTable("dbo.Cities");
            DropTable("dbo.ComplaientDetails");
            DropTable("dbo.BikeCollections");
            DropTable("dbo.BikeCategories");
        }
    }
}
