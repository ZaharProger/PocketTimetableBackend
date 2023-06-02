using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PocketTimetableBackend.Models.Db
{
    [Table("Groups")]
    public class Group: BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public long Id { get; set; }
        public long UniversityId { get; set; }
        public University? University { get; set; }
        public int UrlId { get; set; }
    }
}
