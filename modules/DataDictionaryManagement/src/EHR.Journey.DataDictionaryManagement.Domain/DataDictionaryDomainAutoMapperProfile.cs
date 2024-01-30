namespace EHR.Journey.DataDictionaryManagement
{
    public class DataDictionaryDomainAutoMapperProfile : Profile
    {
        public DataDictionaryDomainAutoMapperProfile()
        {
            CreateMap<DataDictionary, DataDictionaryDto>();
            CreateMap<DataDictionaryDetail, DataDictionaryDetailDto>();
        }
    }
}