using Microsoft.EntityFrameworkCore;

namespace UserManagment.Data
{
    public class UserManagmentDataContext : DbContext
    {
        public UserManagmentDataContext(DbContextOptions<UserManagmentDataContext> options)
            : base(options)
        {

        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Group> Groups => Set<Group>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.NameIdentifier).IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}
