namespace RequestsForRights.Models.Models
{
    public class RequestUserRightModel
    {
        public int IdResourceRight { get; set; }
        public string ResourceRightName { get; set; }
        public int IdResourceRightGrantType { get; set; }
        public string ResourceRightGrantTypeName { get; set; }
        public string Description { get; set; }
    }
}