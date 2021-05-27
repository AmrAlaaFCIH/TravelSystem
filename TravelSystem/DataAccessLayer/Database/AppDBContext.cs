using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelSystem.DataAccessLayer.Models;

namespace TravelSystem.DataAccessLayer.Database
{
    public class AppDBContext : IdentityDbContext<ApplicationUser>
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TripPost> TripPosts {get;set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<TripPost>(trip =>
            {
                trip.Property(p => p.Likes).HasDefaultValue(0);
                trip.Property(p => p.DisLikes).HasDefaultValue(0);
                trip.Property(p => p.Accepted).HasDefaultValue(false);
            });

            builder.Entity<ApplicationUser>().HasMany(user => user.LikedPosts)
                .WithOne(trip => trip.LikedPost)
                .HasForeignKey(trip => trip.LikedPostID)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<ApplicationUser>().HasMany(user => user.DislikedPosts)
                .WithOne(trip => trip.DislikedPost)
                .HasForeignKey(trip => trip.DislikedPostID)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<ApplicationUser>().HasMany(user => user.SavedPosts)
                .WithOne(trip => trip.SavedPost)
                .HasForeignKey(trip => trip.SavedPostID)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<ApplicationUser>().HasMany(user => user.Posted)
                .WithOne(trip => trip.Owner)
                .HasForeignKey(trip => trip.OwnerID)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
