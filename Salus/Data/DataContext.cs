using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Salus.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Auth> auths { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Following> followings { get; set; }
        public DbSet<Food> foods { get; set; }
        public DbSet<Last24h> last24Hs { get; set; }
        public DbSet<Oil> oils { get; set; }
        public DbSet<Recipe> recipes { get; set; }
        public DbSet<Tag> tags { get; set; }
        public DbSet<UserProfile> userProfiles { get; set; }


        private readonly IConfiguration _configuration;

        readonly string connectionString;

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
#if DEBUG
            connectionString = _configuration.GetConnectionString("LocalDB");
#else
            connectionString = _configuration.GetConnectionString("MyAspDB");
#endif
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)
);
#else
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseSqlServer(connectionString);
#endif
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //zero-or-one-to-one relationships
            modelBuilder.Entity<Auth>()
                .HasOne<UserProfile>(a => a.userProfile)
                .WithOne(ad => ad.auth)
                .HasForeignKey<UserProfile>(ad => ad.authOfProfileId);

            modelBuilder.Entity<UserProfile>()
                .HasOne<Last24h>(u => u.last24h)
                .WithOne(l24h => l24h.userProfile)
                .HasForeignKey<Last24h>(l24h => l24h.userProfileId);

            //zero-or-one-to-many relationships
            modelBuilder
                .Entity<Recipe>()
                .HasOne<Oil>(r => r.oil)
                .WithMany(o => o.recipes);

            modelBuilder
                .Entity<Food>()
                .HasOne<Last24h>(f => f.last24h)
                .WithMany(l24h => l24h.foods);

            //many-to-many unable duplicate relationships
            //self join
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


            //normal many-to-many
            modelBuilder
                .Entity<UsersLikeRecipes>()
                .HasKey(ur => new { ur.userId, ur.recipeId });

            modelBuilder.Entity<UsersLikeRecipes>()
                .HasOne(ur => ur.user)
                .WithMany(u => u.likedRecipes)
                .HasForeignKey(ur => ur.recipeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UsersLikeRecipes>()
                .HasOne(ur => ur.recipe)
                .WithMany(r => r.usersWhoLiked)
                .HasForeignKey(ur => ur.userId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder
                .Entity<UsersPreferTags>()
                .HasKey(ut => new { ut.userId, ut.tagId });

            modelBuilder.Entity<UsersPreferTags>()
                .HasOne(ut => ut.user)
                .WithMany(u => u.preferredTags)
                .HasForeignKey(ut => ut.tagId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UsersPreferTags>()
                .HasOne(ut => ut.tag)
                .WithMany(t => t.usersWhoPrefer)
                .HasForeignKey(ut => ut.userId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder
                .Entity<FoodsHaveTags>()
                .HasKey(ft => new { ft.foodId, ft.tagId });

            modelBuilder.Entity<FoodsHaveTags>()
                .HasOne(ft => ft.food)
                .WithMany(f => f.tags)
                .HasForeignKey(ut => ut.tagId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FoodsHaveTags>()
                .HasOne(ut => ut.tag)
                .WithMany(t => t.foodsThatHave)
                .HasForeignKey(ut => ut.foodId)
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
                .HasKey(ri => ri.id);

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
