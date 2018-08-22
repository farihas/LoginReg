using Microsoft.EntityFrameworkCore;

namespace LoginReg.Models
{
    public class MyDbContext : DbContext
    {
        // base() calls the parent class's constructor, passing along the "options" parameter
        // we receive a DbContextOptions as a service which we need to pass to the 
        // parent (base) object here by calling the parent constructor
        // we add services in Startup.cs but we receive services in constructors

        // adding a constructor argument to DbContext type that accepts DbContextOptions<TContext> allows us 
        // to use DBContext (type and options) with dependency injection
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) {}

        // This DbSet contains "User" objects and is called "Users"        
        public DbSet<User> Users { get; set; }
    }
}