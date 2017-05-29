using System;

namespace RequestsForRights.Web.Models.Models
{
    public class ResourceOperatorModel
    {
        public string Department { get; set; }
        public int IdResource { get; set; }
        public string ResourceName { get; set; }
        public string ResourceDescription { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Post { get; set; }
        public string ActType { get; set; }
        public string ActName { get; set; }
        public string ActNumber { get; set; }
        public DateTime? ActDate { get; set; }
        public int? IdFile { get; set; }
    }
}