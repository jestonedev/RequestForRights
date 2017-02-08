using System.Collections.Generic;
using System.Web;

namespace RequestsForRights.Models.Models
{
    public class ResourceActFilesModel
    {
        public IList<HttpPostedFileBase> ResourceAuthorityActs { get; set; }
        public IList<HttpPostedFileBase> ResourceOperatorActs { get; set; }
        public IList<HttpPostedFileBase> ResourceUsingActs { get; set; }
        public IList<ResourcePersonActFilesModel> ResourceOwnerPersons { get; set; }
        public IList<ResourcePersonActFilesModel> ResourceOperatorPersons { get; set; }
    }
}