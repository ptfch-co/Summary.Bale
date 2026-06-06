namespace Summary.Bale
{
    using Core.Security.Permissions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    public class Permissions : IPermissionProvider
    {
        internal static Permission ManageBaleSettings =
            new Permission(nameof(ManageBaleSettings), "Manage Bale Settings");

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[] { ManageBaleSettings }.AsEnumerable());
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new []{ ManageBaleSettings }
                }
            };
        }
    }
}