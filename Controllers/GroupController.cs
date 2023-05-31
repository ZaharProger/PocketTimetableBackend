using Microsoft.AspNetCore.Mvc;
using PocketTimetableBackend.Models.Db;
using PocketTimetableBackend.Models.Http;
using PocketTimetableBackend.Services;

namespace PocketTimetableBackend.Controllers
{
    [Route("pocket-timetable/api/[controller]")]
    [ApiController]
    public class GroupController: ControllerBase
    {
        private readonly GroupService service;

        public GroupController(GroupService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<JsonResult> GetGroupsByUniversityId([FromQuery] long universityId)
        {
            var foundGroups = await service.GetGroupsByUniversityId(universityId);

            return new JsonResult(
                new ControllerResponse<Group>()
                {
                    Data = foundGroups,
                    Success = foundGroups.Length != 0
                }
            );
        }
    }
}
