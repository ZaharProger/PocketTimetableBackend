using PocketTimetableBackend.Models.Db;

namespace PocketTimetableBackend.Models.Http
{
    public class ParseResult
    {
        public University University { get; set; }
        public Group[] Groups { get; set; } = Array.Empty<Group>();
    }
}
