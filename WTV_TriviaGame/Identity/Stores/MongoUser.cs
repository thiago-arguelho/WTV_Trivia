using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WTV_TriviaGame.Identity.Stores
{
    /// <summary>
    /// User managing: 
    /// - IUserStore for basic user managing
    /// - IUserPasswordStore to hashed passwords
    /// - IUserRoleStore to add roles to the user
    /// - IIdentityUserRole as a user and role connection
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class MongoUser<TUser, TKey> : IUserStore<TUser>,
                                          IUserPasswordStore<TUser>,
                                          IUserRoleStore<TUser>
        where TUser : IdentityUser<TKey>, IIdentityUserRole
        where TKey : IEquatable<TKey>
    {
        private readonly IMongoCollection<TUser> _users;

        public MongoUser(MongoBuilder mongoBuilder)
        {
            _users = mongoBuilder.GetCollection<TUser>(MongoBuilder.COLLECTION_USERS);
        }
        /// <summary>
        /// Create user on DB
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            await _users.InsertOneAsync(user, cancellationToken: cancellationToken);
            return IdentityResult.Success;
        }
        /// <summary>
        /// Update user on DB
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            await _users.ReplaceOneAsync(x => x.Id.Equals(user.Id), user, cancellationToken: cancellationToken);
            return IdentityResult.Success;
        }
        /// <summary>
        /// Remove user from DB
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            await _users.DeleteOneAsync(x => x.Id.Equals(user.Id), cancellationToken);
            return IdentityResult.Success;
        }
        /// <summary>
        /// Get UserId
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken) => Task.FromResult(user.Id.ToString());
        /// <summary>
        /// Get username
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken) => Task.FromResult(user.UserName);
        /// <summary>
        /// Sets new UserName
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }
        /// <summary>
        /// Remove Special Character from User
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken) => Task.FromResult(user.NormalizedUserName);
        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            //IUserStore Member Not Used
        }
        /// <summary>
        /// Find User by ID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
            => IsObjectId(userId)
            ? _users.Find(x => x.Id.Equals(userId)).FirstOrDefaultAsync(cancellationToken)
            : Task.FromResult<TUser>(null);
        /// <summary>
        /// Verify if provided userId is a valid MongoDb ObjectId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool IsObjectId(string userId)
        {
            ObjectId temp;
            return ObjectId.TryParse(userId, out temp);
        }
        /// <summary>
        /// Find User by Name
        /// </summary>
        /// <param name="normalizedUserName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
            => _users.Find(x => x.NormalizedUserName == normalizedUserName).FirstOrDefaultAsync();
        /// <summary>
        /// Set User Password Hash
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordHash"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }
        /// <summary>
        /// Get User Password Hash
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken) => Task.FromResult(user.PasswordHash);
        /// <summary>
        /// HasPasswordAsync
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken) => Task.FromResult(false);
        /// <summary>
        /// Add User Role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            user.AddRole(roleName);
            return Task.CompletedTask;
        }
        /// <summary>
        /// Remove User from Role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            user.RemoveRole(roleName);
            return Task.CompletedTask;
        }
        /// <summary>
        /// Get User Roles
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            var roles = user.Roles?.ToArray() ?? Array.Empty<string>();
            return await Task.FromResult(roles);
        }
        /// <summary>
        /// Check if user is in the provided Role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            var roles = await GetRolesAsync(user, cancellationToken);
            return roles.Contains(roleName);
        }
        /// <summary>
        /// Get all users in the provided Role
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
