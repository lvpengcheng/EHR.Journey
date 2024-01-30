namespace EHR.Journey.FileManagement.Files;

public class FileManager : DomainService, IFileManager
{
    private readonly IFileRepository _fileRepository;

    public FileManager(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public virtual async Task CreateAsync(string fileName, string filePath)
    {
        Check.NotNullOrWhiteSpace(fileName, nameof(fileName));
        Check.NotNullOrWhiteSpace(filePath, nameof(filePath));
        var entity = new File(GuidGenerator.Create(), CurrentTenant.Id, fileName, filePath);
        await _fileRepository.InsertAsync(entity);
    }

    public virtual async Task<List<File>> PagingAsync(
        string filter = null,
        int maxResultCount = 10,
        int skipCount = 0)
    {
        return await _fileRepository.GetPagingListAsync(filter, maxResultCount, skipCount);
    }


    public virtual async Task<long> CountAsync(string filter = null)
    {
        return await _fileRepository.GetPagingCountAsync(filter);
    }
}