using PocketTimetableBackend.Models.Db;
using PocketTimetableBackend.Models.Http;
using System.Text.Json.Serialization;

namespace PocketTimetableBackend.Models
{
    [JsonDerivedType(typeof(BaseEntity), typeDiscriminator: "baseEntity")]
    [JsonDerivedType(typeof(Group), typeDiscriminator: "group")]
    [JsonDerivedType(typeof(University), typeDiscriminator: "university")]
    [JsonDerivedType(typeof(StudyDay), typeDiscriminator: "studyDay")]
    [JsonDerivedType(typeof(Subject), typeDiscriminator: "subject")]
    public class BaseEntity
    {
        public string Name { get; set; }
    }
}
