namespace EHR.Journey.NotificationManagement.Hubs
{
    [HubRoute("SignalR/Notification")]
    [Authorize]
    [DisableAuditing]
    public class NotificationHub : AbpHub<INotificationHub>
    {
        
    }
}