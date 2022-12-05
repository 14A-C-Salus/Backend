namespace Salus.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Auth> auths => Set<Auth>();
        public DbSet<UserProfile> userProfiles => Set<UserProfile>();
        public DbSet<Following> followings => Set<Following>();
        public DbSet<Comment> comments => Set<Comment>();

        private readonly IConfiguration _configuration;

        readonly string connectionString;

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = GetConnectionString(_configuration);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder
                .UseSqlServer(connectionString);
        }

        public static string GetConnectionString(IConfiguration config)
        {
            if (config.GetSection("Host:Use").Value == "BsiteDB")
                return config.GetConnectionString("BsiteDB");
            if (config.GetSection("Host:Use").Value == "LocalDB")
                return config.GetConnectionString("LocalDB");
            else if (config.GetSection("Host:Use").Value == "MyAspDB")
                return config.GetConnectionString("MyAspDB");
            else
                throw new Exception("Invalid option in appsettings in 'Host:Use' value.");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //one-to-one
            modelBuilder.Entity<Auth>()
                .HasOne<UserProfile>(a => a.userProfile)
                .WithOne(ad => ad.auth)
                .HasForeignKey<UserProfile>(ad => ad.authOfProfileId);

            //many-to-many unable duplicate
            modelBuilder
                .Entity<Following>()
                .HasKey(fu => new { fu.followedId, fu.followerId });
            modelBuilder.Entity<Following>()
                .HasOne(fu => fu.follower)
                .WithMany(f => f.followerUserProfileToUserProfiles)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Following>()
                .HasOne(fu => fu.followed)
                .WithMany(f => f.followedUserProfileToUserProfiles)
                .OnDelete(DeleteBehavior.Restrict);
            //many-to-many
            modelBuilder
                .Entity<Comment>()
                .HasKey(c => c.id);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.commentFrom)
                .WithMany(c => c.commenterUserProfileToUserProfiles)
                .HasForeignKey(c => c.fromId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.commentTo)
                .WithMany(c => c.commentedUserProfileToUserProfiles)
                .HasForeignKey(c => c.toId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
