using EHR.Journey.BasicManagement.Settings.Dtos;
using Volo.Abp.SettingManagement;

namespace EHR.Journey.BasicManagement.Settings
{
    [Authorize(policy: BasicManagementPermissions.SystemManagement.Setting)]
    public class SettingAppService : BasicManagementAppService, ISettingAppService
    {
        private readonly ISettingDefinitionManager _settingDefinitionManager;
        private readonly ISettingManager _settingManager;
        private readonly IStringLocalizerFactory _factory;

        public SettingAppService(
            ISettingDefinitionManager settingDefinitionManager,
            ISettingManager settingManager,
            IStringLocalizerFactory factory)
        {
            _settingDefinitionManager = settingDefinitionManager;
            _settingManager = settingManager;
            _factory = factory;
        }

        public virtual async Task<List<SettingOutput>> GetAsync()
        {
            var allSettings = (await _settingDefinitionManager.GetAllAsync()).ToList();
            var settings = allSettings
                .Where(e => e.Properties.ContainsKey(BasicManagementSettings.Group.Default)).ToList();

            var settingOutput = settings
                .GroupBy(e => e.Properties[BasicManagementSettings.Group.Default].ToString()).Select(s =>
                    new SettingOutput
                    {
                        Group = s.Key,
                        GroupDisplayName = _factory.CreateDefaultOrNull()[s.Key]
                    }).ToList();

            foreach (var item in settingOutput)
            {
                var currentSettings = settings.Where(e => e.Properties.ContainsValue(item.Group));
                foreach (var itemDefinition in currentSettings)
                {
                    var value = await SettingProvider.GetOrNullAsync(itemDefinition.Name);
                    var type = itemDefinition.Properties
                        .FirstOrDefault(f => f.Key == JourneySettingConsts.ControlType.Default).Value
                        .ToString();

                    item.SettingItemOutput.Add(new SettingItemOutput(
                        itemDefinition.Name,
                        itemDefinition.DisplayName?.Localize(_factory),
                        value,
                        type,
                        itemDefinition.Description?.Localize(_factory)));
                }
            }

            return await Task.FromResult(settingOutput);
        }

        public virtual async Task UpdateAsync(UpdateSettingInput input)
        {
            foreach (var kv in input.Values)
            {
                // The key of the settingValues is in camel_Case, like "setting_Abp_Localization_DefaultLanguage",
                // change it to "Abp.Localization.DefaultLanguage" form
                if (!kv.Key.StartsWith(JourneySettingConsts.Prefix))
                {
                    continue;
                }

                string name = kv.Key.RemovePreFix(JourneySettingConsts.Prefix);
                var setting = await _settingDefinitionManager.GetOrNullAsync(name);
                if (setting == null)
                {
                    continue;
                }

                await SetSetting(setting, kv.Value);
            }
        }

        private Task SetSetting(SettingDefinition setting, string value)
        {
            if (setting.Providers.Any(p => p == UserSettingValueProvider.ProviderName))
            {
                return _settingManager.SetForCurrentUserAsync(setting.Name, value);
            }

            if (setting.Providers.Any(p => p == TenantSettingValueProvider.ProviderName))
            {
                return _settingManager.SetForCurrentTenantAsync(setting.Name, value);
            }

            
            return _settingManager.SetGlobalAsync(setting.Name, value);
        }
    }
}