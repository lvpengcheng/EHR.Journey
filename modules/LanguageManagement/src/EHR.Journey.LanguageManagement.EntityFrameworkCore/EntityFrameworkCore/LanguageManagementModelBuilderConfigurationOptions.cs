namespace EHR.Journey.LanguageManagement.EntityFrameworkCore
{
    public class LanguageManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public LanguageManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}