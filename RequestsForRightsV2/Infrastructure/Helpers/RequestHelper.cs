using System.Text.RegularExpressions;

namespace RequestsForRights.Infrastructure.Helpers
{
    public static class RequestHelper
    {
        public static string PluralRequestState(string requestState)
        {
            requestState = Regex.Replace(requestState, "ая$", "ые");
            return requestState;
        }

        public static string IdRequestTypeToControllerName(int idRequestType)
        {
            switch (idRequestType)
            {
                case 1:
                    return "RequestAddUser";
                case 2:
                    return "RequestModifyPermissions";
                case 3:
                    return "RequestRemoveUser";
                case 4:
                    return "RequestDelegatePermissions";
                default:
                    return null;
            }
        }
    }
}