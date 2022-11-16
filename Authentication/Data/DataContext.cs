namespace Authentication.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Auth> auths => Set<Auth>();
        public DbSet<UserProfile> userProfiles => Set<UserProfile>();
        public DbSet<UserProfileToUserProfile> userProfileToUserProfile => Set<UserProfileToUserProfile>();

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
                .Entity<UserProfileToUserProfile>()
                .HasKey(fu => new { fu.followedId, fu.followerId });
            modelBuilder.Entity<UserProfileToUserProfile>()
                .HasOne<UserProfile>(fu => fu.follower)
                .WithMany(f => f.followerUserProfileToUserProfiles)
                .HasForeignKey(fu => fu.followerId);
            modelBuilder.Entity<UserProfileToUserProfile>()
                .HasOne<UserProfile>(fu => fu.followed)
                .WithMany(u => u.followedUserProfileToUserProfiles)
                .HasForeignKey(fu => fu.followedId);
        }
    }
}
