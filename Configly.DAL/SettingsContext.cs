using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configly.DAL;
using Configly.Entities;

namespace Configly.DAL
{
    public class SettingsContext : DbContext
    {
        static SettingsContext()
        {
            // Without this line, EF6 will break.
            var type = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }

        public SettingsContext()
            : base("Configly")
        {

        }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Property> Propertys { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<PropertyHistory> PropertyHistories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
           
          //  modelBuilder.Entity<Property>().HasRequired(p => p.Setting);
        }
    }
}
