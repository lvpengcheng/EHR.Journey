namespace EHR.Journey
{
    /* Inherit your application services from this class.
     */
    public abstract class JourneyAppService : ApplicationService
    {
        protected JourneyAppService()
        {
            LocalizationResource = typeof(JourneyResource);
        }
    }
}
