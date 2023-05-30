using PocketTimetableBackend.Models.Http;

namespace PocketTimetableBackend.Services.Strategy
{
    public interface IDataParserStrategy
    {
        ParseResult Parse(string targetUri);
    }
}
