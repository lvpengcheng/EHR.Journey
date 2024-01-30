using EHR.Journey.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Employee.Permissions
{
    public class EmployeePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var abpIdentityGroup = context.GetGroup("AbpIdentity");
            var Onboarding = abpIdentityGroup.AddPermission(EmployeePermissions.Employee.Default,
                L("Permission:Employee"), multiTenancySide: MultiTenancySides.Both);
            Onboarding.AddChild(EmployeePermissions.Employee.Create, L("Employee:Create"), multiTenancySide: MultiTenancySides.Both);
            Onboarding.AddChild(EmployeePermissions.Employee.Update, L("Employee:Update"), multiTenancySide: MultiTenancySides.Both);
            Onboarding.AddChild(EmployeePermissions.Employee.Delete, L("Employee:Delete"), multiTenancySide: MultiTenancySides.Both);
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<JourneyLocalizationResource>(name);
        }
    }
}
