using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTV_TriviaGame.Identity.Stores
{
    /// <summary>
    /// TUser Implements IIdentityUserRole
    /// </summary>
    public interface IIdentityUserRole
    {
        public List<string> Roles { get; set; }

        void AddRole(string role);
        void RemoveRole(string role);
    }
}
