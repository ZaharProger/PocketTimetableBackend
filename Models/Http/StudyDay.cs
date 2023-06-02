namespace PocketTimetableBackend.Models.Http
{
    public class StudyDay: BaseEntity
    {
        public string Day { get; set; }
        public Subject[] Subjects { get; set; }
    }
}
