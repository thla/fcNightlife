using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace fcNightlife.Models
{

    public class GoingContext : DbContext
    {

        public GoingContext() : base("GoingContext")
        {
        }

        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }



    public class Location
    {
        public Location()
        {
            Users = new List<User>();
        }
        [Key]
        public string LocationID { get; set; }
        public string City { get; set; }
        public virtual ICollection<User> Users{ get; set; }
    }

    public class User
    {
        [Key]
        [Column(Order = 1)]
        public string UserID { get; set; }
        [Key, ForeignKey("Location")]
        [Column(Order = 2)]
        public string LocationID { get; set; }
        public virtual Location Location { get; set; }
    }
}