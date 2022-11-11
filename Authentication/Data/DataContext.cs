namespace Authentication.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Auth> auths => Set<Auth>();
        public DbSet<UserProfile> userProfiles => Set<UserProfile>();
        private readonly IConfiguration _configuration;

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=salus_DB;User Id=salus_DB;Password=hmw5xto7f8;";
            if (_configuration.GetSection("Host:IsLocalHost").Value == "Yes")
                connectionString = "Server=localhost;Database=master;Trusted_Connection=True;";
            optionsBuilder
                .UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auth>()
                .HasOne<UserProfile>(u => u.userProfile)
                .WithOne(ad => ad.auth)
                .HasForeignKey<UserProfile>(ad => ad.authOfProfileId);
        }
    }
}
