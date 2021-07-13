using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WTV_TriviaGame.Identity.Stores
{
    /// <summary>
    /// Role managing:
    /// - IRoleStore interface and MongoDb as data storage
    /// </summary>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class MongoRole<TRole, TKey> : IRoleStore<TRole>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IMongoCollection<TRole> _roles;

        public MongoRole(MongoBuilder mongoBuilder)
        {
            _roles = mongoBuilder.GetCollection<TRole>(MongoBuilder.COLLECTION_ROLES);
        }
        /// <summary>
        /// Create Role
        /// </summary>
        /// <param name="role"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            await _roles.InsertOneAsync(role, cancellationToken: cancellationToken);
            return IdentityResult.Success;
        }
        /// <summary>
        /// Update Role
        /// </summary>
        /// <param name="role"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken token)
        {
            var result = await _roles.ReplaceOneAsync(r => r.Id.Equals(role.Id), role, cancellationToken: token);

            return IdentityResult.Success;
        }
        /// <summary>
        /// Delete Role
        /// </summary>
        /// <param name="role"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken token)
        {
            var result = await _roles.DeleteOneAsync(r => r.Id.Equals(role.Id), token);

            return IdentityResult.Success;
        }
        /// <summary>
        /// Get Role By Id
        /// </summary>
        /// <param name="role"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
            => Task.FromResult(role.Id.ToString());

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
            => Task.FromResult(role.Name);

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
            => Task.FromResult(role.NormalizedName);

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public virtual Task<TRole> FindByIdAsync(string roleId, CancellationToken token)
            => _roles.Find(r => r.Id.Equals(roleId))
                .FirstOrDefaultAsync(token);

        public virtual Task<TRole> FindByNameAsync(string normalizedName, CancellationToken token)
            => _roles.Find(r => r.NormalizedName == normalizedName)
                .FirstOrDefaultAsync(token);

        public void Dispose()
        {
            //
        }
    }
}
