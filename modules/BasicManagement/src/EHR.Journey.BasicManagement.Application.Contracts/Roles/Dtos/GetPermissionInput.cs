namespace EHR.Journey.BasicManagement.Roles.Dtos
{
    public class GetPermissionInput : IValidatableObject
    {
        public string ProviderName { get; set; }
        public string ProviderKey { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var localization = validationContext.GetRequiredService<IStringLocalizer<JourneyLocalizationResource>>();
            if (ProviderName.IsNullOrWhiteSpace())
            {
                yield return new ValidationResult(
                    localization[JourneyLocalizationErrorCodes.ErrorCode100003, nameof(ProviderName)],
                    new[] { nameof(ProviderName) }
                );
            }

            if (ProviderKey.IsNullOrWhiteSpace())
            {
                yield return new ValidationResult(
                    localization[JourneyLocalizationErrorCodes.ErrorCode100003, nameof(ProviderKey)],
                    new[] { nameof(ProviderKey) }
                );
            }
        }
    }
}