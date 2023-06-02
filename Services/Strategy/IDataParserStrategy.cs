using PocketTimetableBackend.Models;

namespace PocketTimetableBackend.Services.Strategy
{
    public interface IDataParserStrategy
    {
        BaseEntity[] Parse(string targetUri);
    }
}
