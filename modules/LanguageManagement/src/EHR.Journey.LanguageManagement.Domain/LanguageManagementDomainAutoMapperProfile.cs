using EHR.Journey.LanguageManagement.Languages;
using EHR.Journey.LanguageManagement.LanguageTexts;

namespace EHR.Journey.LanguageManagement
{
    public class LanguageManagementDomainAutoMapperProfile : Profile
    {
        public LanguageManagementDomainAutoMapperProfile()
        {
            CreateMap<Language, LanguageDto>();
            CreateMap<LanguageText, LanguageTextDto>();
            CreateMap<Language, LanguageInfo>();
        }
    }
}