using PocketTimetableBackend.Models;
using PocketTimetableBackend.Models.Db;
using RestSharp;
using System.Text.RegularExpressions;

namespace PocketTimetableBackend.Services.Strategy
{
    public class BsuGroupsParserStrategy : IDataParserStrategy
    {
        public BaseEntity[] Parse(string targetUri)
        {
            var restClientOptions = new RestClientOptions(targetUri);
            var restClient = new RestClient(restClientOptions);

            var rawData = restClient.Get(new RestRequest()).Content ?? "";           
            var pattern = new Regex("<a href='\\?idg=(?'groupId'[\\d]+)' class='list-group-item list-group-item-action w-100' data-index='[а-яА-Я-0-9]+'>(?'groupName'[а-яА-Я-0-9]+)");
            var parsedGroups = pattern.Matches(rawData);

            var parsedItems = new BaseEntity[parsedGroups.Count + 1];
            for (int i = 0; i < parsedItems.Length; ++i)
            {
                parsedItems[i] = i == 0 ? new University()
                {
                    Id = 1L,
                    Name = "Байкальский Государственный Университет",
                    ShortName = "БГУ"
                } 
                : 
                new Models.Db.Group()
                {
                    UrlId = int.TryParse(parsedGroups[i - 1].Groups[1].Value, out int resultId) ? resultId : 0,
                    Name = parsedGroups[i - 1].Groups[2].Value,
                    University = (University)parsedItems[0]
                };

            }

            return parsedItems;
        }
    }
}
