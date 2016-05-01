using System.Threading.Tasks;
using Nancy;

namespace TrivialWikiAPI.Roles
{
    public sealed class RolesModule : NancyModule
    {
        private readonly RolesManager rolesManager = new RolesManager();
        public RolesModule()
        {
            Get["/roles", true] = async (param, p) => await GetAllRoles();
        }

        private async Task<Response> GetAllRoles()
        {
            var roles = await rolesManager.GetAllRoles();
            if (roles == null)
            {
                return HttpStatusCode.NoContent;
            }
            return this.Response.AsJson(roles);
        }
    }
}