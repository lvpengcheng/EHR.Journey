namespace EHR.Journey.BasicManagement.Users.Dtos
{
    public class LockUserInput
    {
        public Guid UserId { get; set; }
        
        public bool Locked { get; set; }
    }
}
