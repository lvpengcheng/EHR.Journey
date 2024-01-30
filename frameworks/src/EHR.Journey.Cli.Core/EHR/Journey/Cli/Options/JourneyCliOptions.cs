namespace EHR.Journey.Cli.Options;

public class JourneyCliOptions
{
    /// <summary>
    /// 仓库拥有者
    /// </summary>
    public string Owner { get; set; }
    
    /// <summary>
    /// 仓库Id
    /// </summary>
    public string RepositoryId { get; set; }
    
    /// <summary>
    /// Github Token
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// 解密 Github Token
    /// </summary>
    public string DecryptToken => TokenHelper.Decrypt(Token);
    
    /// <summary>
    /// 模板信息
    /// </summary>
    public List<JourneyTemplateOptions> Templates { get; set; }
 
}