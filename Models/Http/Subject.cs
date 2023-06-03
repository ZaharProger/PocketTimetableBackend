using PocketTimetableBackend.Constants;

namespace PocketTimetableBackend.Models.Http
{
    public class Subject: BaseEntity
    {
        public string Tutor { get; set; }
        public SubjectTypes? SubjectType { get; set; }
        public string Classroom { get; set; }
        public string TimeStart { get; set; }
        public string SubGroupName { get; set; }
    }
}
