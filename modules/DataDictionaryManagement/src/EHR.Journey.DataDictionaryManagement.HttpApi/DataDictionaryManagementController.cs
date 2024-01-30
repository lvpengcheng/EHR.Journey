using EHR.Journey.DataDictionaryManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EHR.Journey.DataDictionaryManagement
{
    public abstract class DataDictionaryManagementController : AbpController
    {
        protected DataDictionaryManagementController()
        {
            LocalizationResource = typeof(DataDictionaryManagementResource);
        }
    }
}
