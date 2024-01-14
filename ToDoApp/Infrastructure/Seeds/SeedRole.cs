using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Data;

namespace Infrastructure.Seeds
{
    public class SeedRole : IDbContextSeed
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public SeedRole(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedAsync(ToDoContext context)
        {
            if (!await _roleManager.RoleExistsAsync("admin"))
            {
                var admin = new IdentityRole<Guid>("admin");
                await _roleManager.CreateAsync(admin);
            }

            if (!await _roleManager.RoleExistsAsync("user"))
            {
                var role = new IdentityRole<Guid>("user");
                await _roleManager.CreateAsync(role);
            }
        }
    }
}
