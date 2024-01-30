namespace EHR.Journey.Extensions.Hangfire
{
    public class CustomHangfireAuthorizeFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var _currentUser = context.GetHttpContext().RequestServices.GetRequiredService<ICurrentUser>();
            return _currentUser.IsAuthenticated;
        }
    }
}
