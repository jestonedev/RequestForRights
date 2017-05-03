using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace RequestsForRights.Domain.Entities
{
    public class RequestUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScriptIgnore]
        public int IdRequestUser { get; set; }

        [MaxLength(256)]
        public string Login { get; set; }

        [Required]
        [MaxLength(512)]
        public string Snp { get; set; }

        [MaxLength(512)]
        public string Post { get; set; }

        [MaxLength(512)]
        public string Phone { get; set; }

        [Required]
        [MaxLength(512)]
        public string Department { get; set; }

        [MaxLength(512)]
        public string Unit { get; set; }

        [MaxLength(512)]
        public string Office { get; set; }

        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual IList<RequestUserAssoc> RequestUserAssoc { get; set; }

        [DefaultValue(false)]
        [ScriptIgnore]
        public bool Deleted { get; set; }

        public override bool Equals(object obj)
        {
            return this == obj as RequestUser;
        }

        protected bool Equals(RequestUser other)
        {
            return IdRequestUser == other.IdRequestUser && string.Equals(Login, other.Login) &&
                   string.Equals(Snp, other.Snp) && string.Equals(Post, other.Post) && string.Equals(Phone, other.Phone) &&
                   string.Equals(Department, other.Department) && string.Equals(Unit, other.Unit) &&
                   string.Equals(Office, other.Office) && Equals(RequestUserAssoc, other.RequestUserAssoc) &&
                   Deleted == other.Deleted;
        }

        public static bool operator==(RequestUser first, RequestUser second)
        {
            if ((object)first == null && (object)second == null)
                return true;
            if ((object)first == null || (object)second == null)
                return false;
            return first.Equals(second);
        }

        public static bool operator !=(RequestUser first, RequestUser second)
        {
            return !(first == second);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IdRequestUser;
                hashCode = (hashCode*397) ^ (Login != null ? Login.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Snp != null ? Snp.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Post != null ? Post.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Phone != null ? Phone.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Department != null ? Department.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Unit != null ? Unit.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Office != null ? Office.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (RequestUserAssoc != null ? RequestUserAssoc.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Deleted.GetHashCode();
                return hashCode;
            }
        }
    }
}
