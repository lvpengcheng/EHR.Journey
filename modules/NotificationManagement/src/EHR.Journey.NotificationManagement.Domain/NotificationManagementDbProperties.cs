namespace EHR.Journey.NotificationManagement
{
    public static class NotificationManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Abp";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "NotificationManagement";
    }
}
