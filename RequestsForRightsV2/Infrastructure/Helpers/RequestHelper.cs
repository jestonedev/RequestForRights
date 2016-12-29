using System.Text.RegularExpressions;

namespace RequestsForRightsV2.Infrastructure.Helpers
{
    public static class RequestHelper
    {
        public static string PluralRequestState(string requestState)
        {
            requestState = Regex.Replace(requestState, "ая$", "ые");
            return requestState;
        }
    }
}