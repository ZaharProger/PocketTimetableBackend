using Microsoft.AspNetCore.Mvc;
using PocketTimetableBackend.Constants;
using PocketTimetableBackend.Models;
using PocketTimetableBackend.Models.Http;
using PocketTimetableBackend.Services;
using PocketTimetableBackend.Services.Strategy;

namespace PocketTimetableBackend.Controllers
{
    [Route("pocket-timetable/api/[controller]")]
    [ApiController]
    public class TimetableController: ControllerBase
    {
        private readonly Parser parser;
        private readonly GroupService service;

        public TimetableController(Parser parser, GroupService service)
        {
            this.parser = parser;
            this.service = service;
        }

        [HttpGet]
        public async Task<JsonResult> GetWeekTimetable([FromQuery] TimetableRequest request)
        {
            string targetUri;
            IDataParserStrategy strategy;
            
            if (request.UniversityId.Equals("1"))
            {
                targetUri = TargetUris.BSU_TIMETABLE_URI;
                strategy = new BsuTimetableParserStrategy();
            }
            else
            {
                targetUri = TargetUris.ISTU_TIMETABLE_URI;
                strategy = new IstuTimetableParserStrategy();
            }

            var timetable = Array.Empty<BaseEntity>();
            var foundGroup = await service
                .GetGroupById(long.TryParse(request.GroupId, out long parsedId) ? parsedId : 0L);
            if (foundGroup != null)
            {
                timetable = parser.Parse($"{targetUri}{foundGroup.UrlId}", strategy);
            }

            return new JsonResult(
                new ControllerResponse<BaseEntity>()
                {
                    Data = timetable,
                    Success = timetable.Length != 0
                }
            );
        }
    }
}
