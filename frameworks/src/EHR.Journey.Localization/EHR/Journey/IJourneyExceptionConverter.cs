namespace EHR.Journey;

public interface IJourneyExceptionConverter
{
    string TryToLocalizeExceptionMessage(Exception exception);
}