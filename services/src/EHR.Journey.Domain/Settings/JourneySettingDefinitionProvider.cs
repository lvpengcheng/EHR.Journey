namespace EHR.Journey.Settings
{
    public class JourneySettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            ConfigEmail(context);
        }

       private static void ConfigEmail(ISettingDefinitionContext context)
        {
            context.GetOrNull(EmailSettingNames.Smtp.Host)
                .WithProperty(JourneySettings.Group.Default,
                    JourneySettings.Group.EmailManagement)
                .WithProperty(JourneySettings.ControlType.Default,
                    JourneySettings.ControlType.TypeText);

            context.GetOrNull(EmailSettingNames.Smtp.Port)
                .WithProperty(JourneySettings.Group.Default,
                    JourneySettings.Group.EmailManagement)
                .WithProperty(JourneySettings.ControlType.Default,
                    JourneySettings.ControlType.Number);

            context.GetOrNull(EmailSettingNames.Smtp.UserName)
                .WithProperty(JourneySettings.Group.Default,
                    JourneySettings.Group.EmailManagement)
                .WithProperty(JourneySettings.ControlType.Default,
                    JourneySettings.ControlType.TypeText);

            context.GetOrNull(EmailSettingNames.Smtp.Password)
                .WithProperty(JourneySettings.Group.Default,
                    JourneySettings.Group.EmailManagement)
                .WithProperty(JourneySettings.ControlType.Default,
                    JourneySettings.ControlType.TypeText);
            

            context.GetOrNull(EmailSettingNames.Smtp.EnableSsl)
                .WithProperty(JourneySettings.Group.Default,
                    JourneySettings.Group.EmailManagement)
                .WithProperty(JourneySettings.ControlType.Default,
                    JourneySettings.ControlType.TypeCheckBox);

            context.GetOrNull(EmailSettingNames.Smtp.UseDefaultCredentials)
                .WithProperty(JourneySettings.Group.Default,
                    JourneySettings.Group.EmailManagement)
                .WithProperty(JourneySettings.ControlType.Default,
                    JourneySettings.ControlType.TypeCheckBox);

            context.GetOrNull(EmailSettingNames.DefaultFromAddress)
                .WithProperty(JourneySettings.Group.Default,
                    JourneySettings.Group.EmailManagement)
                .WithProperty(JourneySettings.ControlType.Default,
                    JourneySettings.ControlType.TypeText);
            
            context.GetOrNull(EmailSettingNames.DefaultFromDisplayName)
                .WithProperty(JourneySettings.Group.Default,
                    JourneySettings.Group.EmailManagement)
                .WithProperty(JourneySettings.ControlType.Default,
                    JourneySettings.ControlType.TypeText);
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<JourneyResource>(name);
        }
    }
}