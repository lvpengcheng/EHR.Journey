namespace EHR.Journey.LanguageManagement.Permissions
{
    public class LanguageManagementPermissions
    {
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(LanguageManagementPermissions));
        }


        //public const string GroupName = "LanguageManagement";
        public const string GroupName = "AbpIdentity";

        public class LanguageTexts
        {
            public const string Default = "AbpIdentity.LanguageTexts";

            public const string Create = "AbpIdentity.LanguageTexts.Create";
            
            public const string Update = "AbpIdentity.LanguageTexts.Update";
        }

        public class Languages
        {
            public const string Default = "AbpIdentity.Languages";

            public const string Update = "AbpIdentity.Languages.Update";

            public const string Create = "AbpIdentity.Languages.Create";

            public const string ChangeDefault = "AbpIdentity.Languages.ChangeDefault";

            public const string Delete = "AbpIdentity.Languages.Delete";
        }
    }
}