namespace EHR.Journey.BasicManagement.Settings.Dtos
{
    public class UpdateSettingInput
    {
        public Dictionary<string, string> Values { get; set; }

        public UpdateSettingInput()
        {
            Values = new Dictionary<string, string>();
        }
    }
}