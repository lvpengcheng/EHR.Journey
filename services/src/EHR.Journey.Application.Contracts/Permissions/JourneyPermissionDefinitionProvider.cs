namespace EHR.Journey.Permissions
{
    public class JourneyPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<JourneyResource>(name);
        }
    }
}