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
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AppDBContext context;

        public DataControl(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager,SignInManager<ApplicationUser> signInManager,AppDBContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.context = context;
        }


        public List<ApplicationUser> GetAllUsers()
        {
            return context.Users
                .Include(e => e.Posted)
                .Include(e=>e.LikedPosts)
                .ThenInclude(e=>e.post)
                .Include(e=>e.DislikedPosts)
                .ThenInclude(e=>e.post)
                .Include(e=>e.SavedPosts)
                .ThenInclude(e=>e.post)
                .Where(e => e.UserName != "Admin")
                .ToList();
        }
        

        public async Task<List<ApplicationUser>> GetAllTravelers()
        {
            var TravelersWithAllInfo = new List<ApplicationUser>();
            var Travelers = await userManager.GetUsersInRoleAsync("Traveler");
            foreach(var traveler in Travelers)
            {
                TravelersWithAllInfo.AddRange(
                    context.Users
                    .Include(e => e.LikedPosts)
                    .ThenInclude(e => e.post)
                    .Include(e => e.DislikedPosts)
                    .ThenInclude(e => e.post)
                    .Include(e => e.SavedPosts)
                    .ThenInclude(e => e.post)
                    .Where(e => e.Id == traveler.Id)
                   );
            }
            return TravelersWithAllInfo;
        }


        public async Task<List<ApplicationUser>> GetAllAgencies()
        {
            var AgenciesWithAllInfo = new List<ApplicationUser>();
            var Agencies = await userManager.GetUsersInRoleAsync("Agency");
            foreach(var Agency in Agencies)
            {
                AgenciesWithAllInfo.AddRange(
                    context.Users
                    .Include(e => e.Posted)
                    .Where(e => e.Id == Agency.Id)
                   );
            }
            return AgenciesWithAllInfo;
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
                context.TripPosts.RemoveRange(user.Posted);
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
            Post.Likes += 1;
            var updatedPost = context.TripPosts.Attach(Post);
            updatedPost.State = EntityState.Modified;
            context.SaveChanges();
            await userManager.UpdateAsync(FoundUser);
        }
       


    }
}
