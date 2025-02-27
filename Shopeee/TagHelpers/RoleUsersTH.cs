﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Shopeee.Areas.Identity;
using Shopeee.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Shopeee.TagHelpers
{
    [HtmlTargetElement("td", Attributes = "i-role")]
    public class RoleUsersTH : TagHelper
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public RoleUsersTH(UserManager<ApplicationUser> UserManager, RoleManager<IdentityRole> RoleManager)
        {
            userManager = UserManager;
            roleManager = RoleManager;
        }

        [HtmlAttributeName("i-role")]
        public string Role { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> names = new List<string>();
            IdentityRole role = await roleManager.FindByIdAsync(Role);
            List<ApplicationUser> ListOfUsers = userManager.Users.ToList();
            if (role != null)
            {
                foreach (var user in ListOfUsers)
                {
                    if (user != null)
                    {
                        if (await userManager.IsInRoleAsync(user, role.Name))
                            names.Add(user.UserName);
                    }
                }
            }
            output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names));
        }
    }
}

