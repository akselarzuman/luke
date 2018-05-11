namespace Luke.Models
{
    public class LukeModel
    {
        public string IdentityName { get; set; }
        public string IdentityGroup { get; set; }
        public string IdentityTrigger { get; set; }
        public ScheduleType ScheduleType { get; set; }
        public bool IsRepeatForever { get; set; }
        public string CronString { get; set; }
    }
}