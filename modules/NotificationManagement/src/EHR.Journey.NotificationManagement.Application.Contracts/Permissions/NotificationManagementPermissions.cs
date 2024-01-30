namespace EHR.Journey.NotificationManagement.Permissions
{
    public class NotificationManagementPermissions
    {
        public const string GroupName = "NotificationManagement";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(NotificationManagementPermissions));
        }
    }
}