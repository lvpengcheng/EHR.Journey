namespace EHR.Journey.LanguageManagement
{
    public static class LanguageManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Abp";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "LanguageManagement";
    }
}
