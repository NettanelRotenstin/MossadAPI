using System.Collections.Generic;
using MossadAPI.Models;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
 
namespace MossadAPI.DAL
{
    public class DBContextMossadAPI : DbContext
    {
        public DBContextMossadAPI(DbContextOptions<DBContextMossadAPI> options) : base(options)
        {
            Database.EnsureCreated();
        }
 

        public   DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }

        public DbSet<Agant> agants { get; set; }
       
        public DbSet<Target> targets { get; set; }

        public DbSet<Mission> missions { get; set; }

        public DbSet<position> positions { get; set; }

     }
}


    






    
       