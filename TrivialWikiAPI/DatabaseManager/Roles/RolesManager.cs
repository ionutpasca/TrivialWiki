using DatabaseManager.DatabaseModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace DatabaseManager.Roles
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
