namespace EHR.Journey.FileManagement;

public class FileManagementApplicationAutoMapperProfile : Profile
{
    public FileManagementApplicationAutoMapperProfile()
    {
        CreateMap<EHR.Journey.FileManagement.Files.File, PagingFileOutput>();
    }
}