using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTV_TriviaGame.Identity.Stores;

namespace WTV_TriviaGame.Models
{
    public class ApplicationUser : IdentityUser<ObjectId>, IIdentityUserRole
    {
        [PersonalData]
        public string Name { get; set; }

        public bool IsAdmin { get; set; }

        public List<string> Roles { get; set; }

        public ApplicationUser()
        {
            Roles = new List<string>();
        }
        public virtual void AddRole(string role)
        {
            Roles.Add(role);
        }
        public void RemoveRole(string role)
        {
            Roles.Remove(role);
        }
    }
}
