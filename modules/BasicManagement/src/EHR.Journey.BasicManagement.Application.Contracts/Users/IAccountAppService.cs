using EHR.Journey.BasicManagement.Users.Dtos;

namespace EHR.Journey.BasicManagement.Users
{
    public interface IAccountAppService: IApplicationService
    {
        /// <summary>
        /// 用户名密码登录
        /// </summary>
        Task<LoginOutput> LoginAsync(LoginInput input);
    }
}
