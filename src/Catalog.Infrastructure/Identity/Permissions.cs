namespace Catalog.Infrastructure.Identity
{
    public static class Permissions
    {
        public static List<string> GetPermissionsForModule(string module)
        {
            return new List<string>
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.Read",
                $"Permissions.{module}.Update",
                $"Permissions.{module}.Delete"
            };
        }
    }
}
