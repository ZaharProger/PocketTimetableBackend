using PocketTimetableBackend.Models;
using PocketTimetableBackend.Services.Strategy;

namespace PocketTimetableBackend.Services
{
    public class Parser
    {
        public BaseEntity[] Parse(string targetUri, IDataParserStrategy strategy)
        {
            return strategy.Parse(targetUri);
        }
    }
}
