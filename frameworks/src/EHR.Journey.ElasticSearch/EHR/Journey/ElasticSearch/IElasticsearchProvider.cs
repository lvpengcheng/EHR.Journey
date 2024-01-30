namespace EHR.Journey.ElasticSearch;

public interface IElasticsearchProvider
{
    IElasticClient GetClient();
}