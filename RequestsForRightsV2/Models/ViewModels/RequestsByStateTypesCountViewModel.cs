using RequestsForRights.Domain.Entities;

namespace RequestsForRights.Web.Models.ViewModels
{
    public class RequestsCountByStateTypesViewModel
    {
        public RequestStateType RequestStateType { get; set; }
        public int RequestCount { get; set; }
    }
}