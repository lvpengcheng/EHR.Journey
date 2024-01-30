namespace EHR.Journey.DataDictionaryManagement
{
    public static class DataDictionaryManagementDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Abp";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "DataDictionaryManagement";
    }
}
