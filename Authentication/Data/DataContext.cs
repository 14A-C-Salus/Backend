namespace Authentication.Data
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                //.UseSqlServer("Server=sql.bsite.net\\MSSQL2016;Database=salus_DB;User Id=salus_DB;Password=hmw5xto7f8;");
                .UseSqlServer("Server=localhost;Database=master;Trusted_Connection=True;");
        }
        public DbSet<Auth> auths => Set<Auth>();
        public DbSet<UserProfile> userProfiles => Set<UserProfile>();
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auth>()
                .HasOne<UserProfile>(u => u.userProfile)
                .WithOne(ad => ad.auth)
                .HasForeignKey<UserProfile>(ad => ad.authOfProfileId);
        }
    }

}
