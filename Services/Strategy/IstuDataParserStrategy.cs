using PocketTimetableBackend.Models.Http;
using RestSharp;
using System.Text.RegularExpressions;

namespace PocketTimetableBackend.Services.Strategy
{
    public class IstuDataParserStrategy : IDataParserStrategy
    {
        public ParseResult Parse(string targetUri)
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

            var parseResult = new ParseResult();
            parseResult.University = new()
            {
                Id = 2L,
                Name = "Иркутский Национальный Исследовательский Технический Университет",
                ShortName = "ИрНИТУ"
            };
            parseResult.Groups = parsedGroups
                .Select(parsedGroup => new Models.Db.Group()
                {
                    UrlId = int.TryParse(parsedGroup.Groups[1].Value, out int resultId) ? resultId : 0,
                    Name = parsedGroup.Groups[2].Value,
                    University = parseResult.University
                })
                .ToArray();

            return parseResult;
        }
    }
}
