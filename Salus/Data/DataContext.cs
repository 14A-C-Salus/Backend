﻿using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Salus.Models;

namespace Salus.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Auth> auths { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<Diet> diets { get; set; }
        public DbSet<Following> followings { get; set; }
        public DbSet<Last24h> last24Hs { get; set; }
        public DbSet<Oil> oils { get; set; }
        public DbSet<Recipe> recipes { get; set; }
        public DbSet<Tag> tags { get; set; }
        public DbSet<UserProfile> userProfiles { get; set; }


        private readonly IConfiguration _configuration;

        readonly string connectionString;
        // Reaseon: Properties need to make datebase, but we never use them in code.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable. 
        public DataContext(IConfiguration configuration)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
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
            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString));
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
                .HasOne<Oil>(r => r.oil)
                .WithMany(o => o.recipes);

            modelBuilder
                .Entity<UserProfile>()
                .HasOne<Diet>(u => u.diet)
                .WithMany(d => d.userProfiles);

            modelBuilder.Entity<Recipe>()
                .HasOne<UserProfile>(r => r.userProfile)
                .WithMany(up => up.recipes);

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
                .HasForeignKey(ur => ur.userId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UsersLikeRecipes>()
                .HasOne(ur => ur.recipe)
                .WithMany(r => r.usersWhoLiked)
                .HasForeignKey(ur => ur.recipeId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder
                .Entity<UsersPreferTags>()
                .HasKey(ut => new { ut.userId, ut.tagId });

            modelBuilder.Entity<UsersPreferTags>()
                .HasOne(ut => ut.user)
                .WithMany(u => u.preferredTags)
                .HasForeignKey(ut => ut.userId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UsersPreferTags>()
                .HasOne(ut => ut.tag)
                .WithMany(t => t.usersWhoPrefer)
                .HasForeignKey(ut => ut.tagId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder
                .Entity<RecepiesHaveTags>()
                .HasKey(ft => new { ft.recipeId, ft.tagId });

            modelBuilder.Entity<RecepiesHaveTags>()
                .HasOne(ft => ft.tag)
                .WithMany(t => t.recepiesThatHave)
                .HasForeignKey(ft => ft.tagId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RecepiesHaveTags>()
                .HasOne(ft => ft.recipe)
                .WithMany(f => f.tags)
                .HasForeignKey(ft => ft.recipeId)
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
                .HasOne(ri => ri.recipe)
                .WithMany(r => r.ingredients)
                .HasForeignKey(ri => ri.recipeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RecipesIncludeIngredients>()
                .HasOne(ri => ri.ingredient)
                .WithMany(r => r.recipes)
                .HasForeignKey(ri => ri.ingredientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Last24h>()
                .HasKey(l24h => l24h.id);

            modelBuilder
                .Entity<Last24h>()
                .HasOne<Recipe>(l24h => l24h.recipe)
                .WithMany(r => r.last24hs)
                .HasForeignKey(l24h => l24h.recipeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Last24h>()
                .HasOne<UserProfile>(l24h => l24h.userProfile)
                .WithMany(up => up.last24hs)
                .HasForeignKey(l24h => l24h.userProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
