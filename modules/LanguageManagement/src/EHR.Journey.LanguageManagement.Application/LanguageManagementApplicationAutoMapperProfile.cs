using EHR.Journey.LanguageManagement.Languages;
using EHR.Journey.LanguageManagement.LanguageTexts;

namespace EHR.Journey.LanguageManagement
{
    public class LanguageManagementApplicationAutoMapperProfile : Profile
    {
        public LanguageManagementApplicationAutoMapperProfile()
        {
            CreateMap<LanguageDto, PageLanguageOutput>();
            CreateMap<Language, PageLanguageOutput>();
            CreateMap<LanguageTextDto, PageLanguageTextOutput>();
        }
    }
}