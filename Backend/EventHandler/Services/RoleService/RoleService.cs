using EventHandler.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace EventHandler.Services.RoleService
{
    public class RoleService
    {
        private readonly UserManager<AppUser> _userManager;

        public RoleService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> AssignOrganizerRole(AppUser user)
        {
            
            // Check if the user already has the "Organiser" role
            var isInRole = await _userManager.IsInRoleAsync(user, "Organiser");
            if (isInRole)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User is already in the Organiser role." });
            }

            // Assign the "Organiser" role to the user
            return await _userManager.AddToRoleAsync(user, "Organiser");
            
        }
    
    }

}
