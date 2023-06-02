using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PocketTimetableBackend.Models.Db
{
    [Table("Universities")]
    public class University: BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None), Key]
        public long Id { get; set; }
        public string ShortName { get; set; }
        public List<Group> Groups { get; set; } = new();
    }
}
