namespace ProductIdentification.Core.Models.Roles
{
    public class CustomRoles
    {
        public const string ManagerOrAbove = Role.Manager + ", " + Role.Admin;
        public const string DataManagerOrAbove = Role.DataManager + ", " + Role.Admin;
    }
}