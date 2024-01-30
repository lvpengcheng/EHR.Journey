namespace EHR.Journey.DataDictionaryManagement.DataDictionaries.Dtos
{
    public class UpdateDetailInput : IValidatableObject
    {
        public Guid DataDictionaryId { get; set; }

        public Guid Id { get; set; }

        public string DisplayText { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var localization = validationContext.GetRequiredService<IStringLocalizer<JourneyLocalizationResource>>();
            
            if (DisplayText.IsNullOrWhiteSpace())
            {
                yield return new ValidationResult(
                    localization[JourneyLocalizationErrorCodes.ErrorCode100003, nameof(DisplayText)],
                    new[] { nameof(DisplayText) }
                );
            }
        }
    }
}