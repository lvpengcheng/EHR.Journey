namespace EHR.Journey.Users
{
    public interface IUserFreeSqlBasicRepository
    {
        Task<List<UserOutput>> GetListAsync();
    }
}
