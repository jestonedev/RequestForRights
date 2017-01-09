using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequestsForRights.Domain.Helpers;
using RequestsForRights.Domain.Interfaces;

namespace RequestsForRights.Domain.Entities
{
    public class ResourceGroup : IStringMatchable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdResourceGroup { get; set; }
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }
        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public bool Match(string value)
        {
            return MatchHelper.MatchValueInsensitive(Name, value) ||
                   MatchHelper.MatchValueInsensitive(Description, value);
        }
    }
}
