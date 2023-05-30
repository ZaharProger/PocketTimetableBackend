using PocketTimetableBackend.Models.Http;
using RestSharp;
using System.Text.RegularExpressions;

namespace PocketTimetableBackend.Services.Strategy
{
    public class BsuDataParserStrategy : IDataParserStrategy
    {
        public ParseResult Parse(string targetUri)
        {
            var restClientOptions = new RestClientOptions(targetUri);
            var restClient = new RestClient(restClientOptions);

            var rawData = restClient.Get(new RestRequest()).Content ?? "";           
            var pattern = new Regex("<a href='\\?idg=(?'groupId'[\\d]+)' class='list-group-item list-group-item-action w-100' data-index='[а-яА-Я-0-9]+'>(?'groupName'[а-яА-Я-0-9]+)");
            var parsedGroups = pattern.Matches(rawData);

            var parseResult = new ParseResult();
            parseResult.University = new()
            {
                Id = 1L,
                Name = "Байкальский Государственный Университет",
                ShortName = "БГУ"
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
