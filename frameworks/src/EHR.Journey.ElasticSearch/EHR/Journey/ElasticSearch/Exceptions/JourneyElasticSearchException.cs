namespace EHR.Journey.ElasticSearch.Exceptions;

public class JourneyElasticSearchException : BusinessException
{
    public JourneyElasticSearchException(
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

    public JourneyElasticSearchException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context)
    {
    }
}