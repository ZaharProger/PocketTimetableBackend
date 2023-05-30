using PocketTimetableBackend.Models.Http;
using PocketTimetableBackend.Services.Strategy;

namespace PocketTimetableBackend.Services
{
    public class Parser
    {
        public ParseResult Parse(string targetUri, IDataParserStrategy strategy)
        {
            return strategy.Parse(targetUri);
        }
    }
}
