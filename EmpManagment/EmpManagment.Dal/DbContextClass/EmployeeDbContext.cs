using EmpManagment.Bol.Entities;
using EmpManagment.Bol.IdentityEntities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpManagment.Dal.DbContextClass
{
    public class EmployeeDbContext : IdentityDbContext<EmployeeUser>
    {
        public EmployeeDbContext() : base("name=EmployeeDbConnectionString", throwIfV1Schema: false)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
         
            //modelBuilder.Entity<Country>()
            //    .HasRequired(c => c.CountryName)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<State>()
            //    .HasRequired(c => c.Country)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);
            //foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            //}
        }
        public static EmployeeDbContext Create()
        {
            return new EmployeeDbContext();
        }
        public DbSet<Complaients> Complaients { get; set; }
        public DbSet<ComplaientDetails> ComplaientDetails { get; set; }
        public DbSet<ComplaientCategory> ComplaientCategories { get; set; }
        public DbSet<ComplaientPermamentAddress> ComplaientPermamentAddresses { get; set; }
        public DbSet<ComplaientTempAddress> ComplaientTempAddresses { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<BikeCollection> BikeCollections { get; set; }
        public DbSet<BikeCategory> BikeCategories { get; set; }
        public DbSet<Bulk> Bulk { get; set; }
        public DbSet<BulkDatas> BulkDatas { get; set; }
        public DbSet<Files> Files { get; set; }
        
    }
}
