namespace EHR.Journey.ElasticSearch.Exceptions;

public class JourneyElasticSearchEntityNotFoundException : BusinessException
{
    public JourneyElasticSearchEntityNotFoundException(
        string code = null,
        string message = null,
        string details = null,
        Exception innerException = null,
        LogLevel logLevel = LogLevel.Error)
        : base(
            code,
            message,
            details,
            innerException,
            logLevel
        )
    {
    }

    public JourneyElasticSearchEntityNotFoundException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context)
    {
    }
}