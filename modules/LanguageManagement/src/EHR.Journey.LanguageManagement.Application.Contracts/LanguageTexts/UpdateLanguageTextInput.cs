namespace EHR.Journey.LanguageManagement.LanguageTexts;

/// <summary>
/// 删除语言文本
/// </summary>
public class UpdateLanguageTextInput
{
    /// <summary>
    /// 资源名称
    /// </summary>
    [Required(ErrorMessage = "资源名称不能为空")]
    public string ResourceName { get; set; }

    /// <summary>
    /// 语言名称
    /// </summary>
    [Required(ErrorMessage = "语言名称不能为空")]
    public string CultureName { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Required(ErrorMessage = "名称不能为空")]
    public string Name { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    [Required(ErrorMessage = "值不能为空")]
    public string Value { get; set; }
}