namespace Authentication.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Auth> auths => Set<Auth>();
        public DbSet<UserProfile> userProfiles => Set<UserProfile>();
        public DbSet<Follow> follows => Set<Follow>();
        public DbSet<FollowUserProfile> followUserProfiles => Set<FollowUserProfile>();

        private readonly IConfiguration _configuration;

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            string connectionString = _configuration.GetConnectionString("BsiteDB");
            if (_configuration.GetSection("Host:Use").Value == "LocalDB")
                connectionString = _configuration.GetConnectionString("LocalDB");
            else if (_configuration.GetSection("Host:Use").Value == "MyAspDB")
                connectionString = _configuration.GetConnectionString("MyAspDB");

            optionsBuilder
                .UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //one-to-one
            modelBuilder.Entity<Auth>()
                .HasOne<UserProfile>(a => a.userProfile)
                .WithOne(ad => ad.auth)
                .HasForeignKey<UserProfile>(ad => ad.authOfProfileId);
            
            //many-to-many
            modelBuilder
                .Entity<FollowUserProfile>()
                .HasKey(fu => new { fu.followId, fu.userProfileId });
            modelBuilder.Entity<FollowUserProfile>()
                .HasOne<Follow>(fu => fu.follow)
                .WithMany(f => f.followUserProfile)
                .HasForeignKey(fu => fu.followId);
            modelBuilder.Entity<FollowUserProfile>()
                .HasOne<UserProfile>(fu => fu.userProfile)
                .WithMany(u => u.followUserProfile)
                .HasForeignKey(fu => fu.userProfileId);
        }
    }
}
