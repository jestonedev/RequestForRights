using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestsForRights.Domain.Entities
{
    public class ActFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdFile { get; set; }
        public string FileOriginalName { get; set; }
        public byte[] FileContent { get; set; }
        public string FileContentType { get; set; }
    }
}
