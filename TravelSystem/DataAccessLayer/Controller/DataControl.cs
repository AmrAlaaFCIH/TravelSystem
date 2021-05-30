using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelSystem.DataAccessLayer.Database;
using TravelSystem.DataAccessLayer.Models;

namespace TravelSystem.DataAccessLayer.Controller
{
    public class DataControl : IDataControl
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDBContext context;

        public DataControl(UserManager<ApplicationUser> userManager,AppDBContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }
        
        public async Task<IdentityResult> CreateTraveler(ApplicationUser user,string password)
        {
            var result = await userManager.CreateAsync(user,password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Traveler");
            }
            return result;
        }
        public async Task<IdentityResult> CreateAgency(ApplicationUser user, string password)
        {
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Agency");
            }
            return result;
        }



        public async Task<IdentityResult> DeleteUser(ApplicationUser user)
        {
            if (user.Posted != null || user.Posted.Count != 0)
            {
                var posts=context.TripPosts.Where(e => e.OwnerID == user.Id);
                context.TripPosts.RemoveRange(posts);
                context.SaveChanges();
            }           
            var result = await userManager.DeleteAsync(user);
            return result;
        }

        public void DeleteTrip(Guid Id)
        {

            context.TripPosts.Remove(context.TripPosts.FirstOrDefault(e=>e.Id==Id));
            context.SaveChanges();
        }

        public async Task AddTrip(string userID,TripPost post)
        {
            var FoundUser = await userManager.FindByIdAsync(userID);
            FoundUser.Posted.Add(post);
            await userManager.UpdateAsync(FoundUser);
        }

        public async Task LikeTripPost(string userID,Guid postID)
        {
            var FoundUser = await userManager.FindByIdAsync(userID);
            var Post = context.TripPosts.FirstOrDefault(e => e.Id == postID);
            if (Post != null)
            {
                await context.LikedPosts.AddAsync(new LikedPostTable()
                {
                    user = FoundUser,
                    post = Post
                });
            }
            context.SaveChanges();
            await userManager.UpdateAsync(FoundUser);
        }
       
        public int GetPostLikes(Guid PostID)
        {
            return context.LikedPosts.Where(e => e.postID == PostID).Count();
        }

        public int GetPostDisLikes(Guid PostID)
        {
            return context.dislikedPosts.Where(e => e.postID == PostID).Count();
        }


    }
}
