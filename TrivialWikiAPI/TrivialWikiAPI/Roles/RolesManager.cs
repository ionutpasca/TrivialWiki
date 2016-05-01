
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using TrivialWikiAPI.DatabaseModels;

namespace TrivialWikiAPI.Roles
{
    public sealed class RolesManager 
    {
        public async Task<List<Role>> GetAllRoles()
        {
            using (var databaseContext = new DatabaseContext())
            {
                return await databaseContext.Roles.ToListAsync();
            }
        }
    }
}