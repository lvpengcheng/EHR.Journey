using EHR.Journey.BasicManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EHR.Journey.BasicManagement;

public abstract class BasicManagementController : AbpControllerBase
{
    protected BasicManagementController()
    {
        LocalizationResource = typeof(BasicManagementResource);
    }
}
