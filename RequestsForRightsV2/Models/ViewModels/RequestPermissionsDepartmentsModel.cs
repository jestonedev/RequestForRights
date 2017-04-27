namespace RequestsForRights.Web.Models.ViewModels
{
    public class RequestPermissionsDepartmentsModel
    {
        public int IdDepartment { get; set; }
        public string DepartmentName { get; set; }
        public bool RequestsAllowed { get; set; }
    }
}