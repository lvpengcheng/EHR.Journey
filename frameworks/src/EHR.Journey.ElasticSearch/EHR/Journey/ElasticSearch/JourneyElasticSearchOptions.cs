namespace EHR.Journey.ElasticSearch;

public class JourneyElasticSearchOptions
{
    /// <summary>
    /// es地址
    /// </summary>
    public string Host { get; set; }
    
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
}