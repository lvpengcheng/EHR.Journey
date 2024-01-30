using EHR.Journey.Users;
using EHR.Journey.Users.Dto;

namespace EHR.Journey.FreeSqlRepository
{
    public class UserFreeSqlBasicRepository : FreeSqlBasicRepository, IUserFreeSqlBasicRepository
    {
        public async Task<List<UserOutput>> GetListAsync()
        {
            var sql = "select id from AbpUsers";
            var result = await FreeSql.Select<UserOutput>()
            .WithSql(sql)
            .ToListAsync();
            return result;
        }
    }
}
