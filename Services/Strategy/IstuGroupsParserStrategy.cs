using PocketTimetableBackend.Models;
using PocketTimetableBackend.Models.Db;
using RestSharp;
using System.Text.RegularExpressions;

namespace PocketTimetableBackend.Services.Strategy
{
    public class IstuGroupsParserStrategy : IDataParserStrategy
    {
        public BaseEntity[] Parse(string targetUri)
        {
            var istuInstitutesIds = new short[]
            {
                664, 685, 1, 669, 671, 680,
                683, 684, 4, 144, 10, 688
            };

            var parsedGroups = new List<Match>();
            foreach (var instituteId in istuInstitutesIds)
            {
                var restClientOptions = new RestClientOptions($"{targetUri}{instituteId}");
                var restClient = new RestClient(restClientOptions);

                var rawData = restClient.Get(new RestRequest()).Content ?? "";
                var pattern = new Regex("<a href=\"\\?group=(?'groupId'[\\d]+)\">(?'groupName'[а-яА-Я-0-9]+)");

                parsedGroups.AddRange(pattern.Matches(rawData).ToList());
            }

            var parsedItems = new BaseEntity[parsedGroups.Count + 1];
            for (int i = 0; i < parsedItems.Length; ++i)
            {
                parsedItems[i] = i == 0 ? new University()
                {
                    Id = 2L,
                    Name = "Иркутский Национальный Исследовательский Технический Университет",
                    ShortName = "ИрНИТУ"
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
