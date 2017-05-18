using System.Text.RegularExpressions;

namespace RequestsForRights.Web.Infrastructure.Helpers
{
    public static class RightHelper
    {
        public static string PastTenseRightGrantType(string requestState)
        {
            requestState = Regex.Replace(requestState, "ать право$", "аны права");
            return requestState;
        }
    }
}