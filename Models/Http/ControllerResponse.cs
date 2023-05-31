using PocketTimetableBackend.Models.Db;

namespace PocketTimetableBackend.Models.Http
{
    public class ControllerResponse<T> where T: BaseDbEntity
    {
        public T[] Data { get; set; }
        public bool Success { get; set; }
    }
}
