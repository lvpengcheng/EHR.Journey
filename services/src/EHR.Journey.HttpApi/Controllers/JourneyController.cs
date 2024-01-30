namespace EHR.Journey.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class JourneyController : AbpController
    {
        protected JourneyController()
        {
            LocalizationResource = typeof(JourneyResource);
        }
    }
}