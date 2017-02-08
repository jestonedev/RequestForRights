using System.Collections.Generic;
using System.Web;

namespace RequestsForRights.Models.Models
{
    public class ResourcePersonActFilesModel
    {
        public int IdPerson { get; set; }
        public IList<HttpPostedFileBase> Acts { get; set; }
    }
}