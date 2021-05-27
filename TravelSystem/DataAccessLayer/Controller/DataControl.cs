using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelSystem.DataAccessLayer.Database;
using TravelSystem.DataAccessLayer.Models;

namespace TravelSystem.DataAccessLayer.Controller
{
    public class DataControl 
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

        public async Task<List<ApplicationUser>> GetAllTravelers()
        {
            var users = await userManager.GetUsersInRoleAsync("Traveler");
            return users.ToList();
        }

        public async Task<List<ApplicationUser>> GetAllAgencies()
        {
            var users = await userManager.GetUsersInRoleAsync("Agency");
            return users.ToList();
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
       


    }
}
