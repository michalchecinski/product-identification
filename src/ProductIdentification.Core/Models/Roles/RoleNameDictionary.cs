using System.Collections.Generic;

namespace ProductIdentification.Core.Models.Roles
{
    public static class RoleNameDictionary
    {
        public static Dictionary<string, string> All => new Dictionary<string, string>()
        {
            {Role.Admin, nameof(Role.Admin)},
            {Role.Manager, nameof(Role.Manager)},
            {Role.DataManager, nameof(Role.DataManager)},
            {Role.WarehouseMan, nameof(Role.WarehouseMan)}
        };

        public static Dictionary<string, string> BelowManager => new Dictionary<string, string>()
        {
            {Role.DataManager, nameof(Role.DataManager)},
            {Role.WarehouseMan, nameof(Role.WarehouseMan)}
        };
    }
}