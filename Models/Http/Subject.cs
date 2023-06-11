namespace PocketTimetableBackend.Models.Http
{
    public class Subject: BaseEntity
    {
        public string Tutor { get; set; }
        public string? SubjectType { get; set; }
        public string Classroom { get; set; }
        public int TimeStart { get; set; }
        public int TimeEnd { get; set; }
        public string SubGroupName { get; set; }
    }
}
