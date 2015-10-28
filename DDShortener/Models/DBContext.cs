using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DDShortener.Models
{
    public class DBContext : DbContext
    {
        public DBContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<DBContext>(null);
        }

        public DbSet<URL> Urls { get; set; }
    }
}