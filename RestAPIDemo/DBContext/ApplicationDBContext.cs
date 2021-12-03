using System;
using Microsoft.EntityFrameworkCore;


namespace RestAPIDemo.Models 
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
