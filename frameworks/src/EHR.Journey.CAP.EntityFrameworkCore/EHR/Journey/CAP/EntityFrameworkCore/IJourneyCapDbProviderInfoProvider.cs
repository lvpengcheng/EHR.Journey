namespace EHR.Journey.CAP.EntityFrameworkCore;

public interface IJourneyCapDbProviderInfoProvider
{
    JourneyCapDbProviderInfo GetOrNull(string dbProviderName);
}