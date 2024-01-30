namespace EHR.Journey.LanguageManagement.Languages;

public class LanguageListCacheItem
{
    public List<LanguageInfo> Languages { get; set; }

    public LanguageListCacheItem(List<LanguageInfo> languages)
    {
        this.Languages = languages;
    }
    
    public static string CalculateCacheKey()
    {
        return "AllLanguages";
    }
}