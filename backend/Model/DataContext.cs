using Microsoft.EntityFrameworkCore;

namespace BackEnd.Model
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DataContext()
        {
            
        }
        public DbSet<User>? UserList { get; set; }
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            // modelBuilder.Entity<User>().HasData(
            // new User
            // {
            //     Id = 1,
            //     Username="utest1",
            //     Password = "St9tpNN2zrinRGNUgKWCy4JjZRFEorSQ0Zg3a/8m7k4=" // test1
            // },
            // new User
            // {
            //     Id = 2,
            //     Username="utest2",
            //     Password = "zWoe4T9h2Hj9G4dyUtWwcKwV6zMR1Q0yr3Uch+xSze8=" // test2
            // },
            // new User
            // {
            //     Id = 3,
            //     Username="utest3",
            //     Password = "6RwNz8ehCp0yZ0KkUE7i+Shy+2l7C1Eh9dT/RULwZN8=" // test3
            // }
            //  );

        }
    }
}
