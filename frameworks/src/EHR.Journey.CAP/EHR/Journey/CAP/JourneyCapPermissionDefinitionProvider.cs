namespace EHR.Journey.CAP;

public class JourneyCapPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var abpIdentityGroup = context.GetGroup(JourneyCapPermissions.CapManagement.Default);

        abpIdentityGroup.AddPermission(JourneyCapPermissions.CapManagement.Cap, L("Permission:Cap"), multiTenancySide: MultiTenancySides.Both);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<JourneyLocalizationResource>(name);
    }
}