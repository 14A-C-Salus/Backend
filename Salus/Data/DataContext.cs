namespace Salus.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Auth> auths => Set<Auth>();
        public DbSet<UserProfile> userProfiles => Set<UserProfile>();
        public DbSet<Following> followings => Set<Following>();
        public DbSet<Comment> comments => Set<Comment>();
        public DbSet<Recipe> recipes => Set<Recipe>();
        public DbSet<Food> foods => Set<Food>();
        public DbSet<Oil> oils => Set<Oil>();

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
            //zero-or-one-to-one relationships
            modelBuilder.Entity<Auth>()
                .HasOne<UserProfile>(a => a.userProfile)
                .WithOne(ad => ad.auth)
                .HasForeignKey<UserProfile>(ad => ad.authOfProfileId);

            //zero-or-one-to-many relationships
            modelBuilder
                .Entity<Recipe>()
                .HasOne<Oil>(o => o.oil)
                .WithMany(o => o.recipes);

            //many-to-many unable duplicate relationships
            modelBuilder
                .Entity<Following>()
                .HasKey(fu => new { fu.followedId, fu.followerId });
            
            modelBuilder.Entity<Following>()
                .HasOne(f => f.followed)
                .WithMany(u => u.followers)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Following>()
                .HasOne(f => f.follower)
                .WithMany(u => u.followeds)
                .OnDelete(DeleteBehavior.Restrict);

            //many-to-many relationships
            //self join
            modelBuilder
                .Entity<Comment>()
                .HasKey(c => c.id);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.commentFrom)
                .WithMany(u => u.commenteds)
                .HasForeignKey(c => c.fromId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.commentTo)
                .WithMany(u => u.commenters)
                .HasForeignKey(c => c.toId)
                .OnDelete(DeleteBehavior.Restrict);


            //normal many-to-many
            modelBuilder
                .Entity<RecipesIncludeIngredients>()
                .HasKey(r => r.id);

            modelBuilder.Entity<RecipesIncludeIngredients>()
                .HasOne(ri => ri.food)
                .WithMany(f => f.recipes)
                .HasForeignKey(ri => ri.foodId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RecipesIncludeIngredients>()
                .HasOne(ri => ri.recipe)
                .WithMany(r => r.ingredients)
                .HasForeignKey(ri => ri.recipeId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
