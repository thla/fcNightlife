using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public DbSet<User> Users { get; set; }

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
        public User()
        {
            Locations= new List<Location>();
        }
        [Key]
        public string UserID { get; set; }
        public virtual ICollection<Location> Locations{ get; set; }
    }
}