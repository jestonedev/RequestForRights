using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Models.ModelViews
{
    public class RequestsCountByStateTypesViewModel
    {
        public RequestStateType RequestStateType { get; set; }
        public int RequestCount { get; set; }
    }
}