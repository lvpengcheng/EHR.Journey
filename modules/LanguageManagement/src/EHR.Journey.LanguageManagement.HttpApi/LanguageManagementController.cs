using EHR.Journey.LanguageManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EHR.Journey.LanguageManagement
{
    public abstract class LanguageManagementController : AbpController
    {
        protected LanguageManagementController()
        {
            LocalizationResource = typeof(LanguageManagementResource);
        }
    }
}
