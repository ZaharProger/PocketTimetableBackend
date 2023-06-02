namespace PocketTimetableBackend.Models.Http
{
    public class ControllerResponse<T> where T: BaseEntity
    {
        public T[] Data { get; set; }
        public bool Success { get; set; }
    }
}
