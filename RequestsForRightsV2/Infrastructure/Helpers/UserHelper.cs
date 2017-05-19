using System;
using RequestsForRights.Domain.Enums;
using RequestsForRights.Web.Infrastructure.Enums;

namespace RequestsForRights.Web.Infrastructure.Helpers
{
    public static class UserHelper
    {
        public static string LabelToUserCategory(UsersCategory usersCategory)
        {
            switch (usersCategory)
            {
                case UsersCategory.ActiveUsers:
                    return "Активные";
                case UsersCategory.BlockedUsers:
                    return "Заблокированные";
                case UsersCategory.ActiveAndBlockedUsers:
                    return "Активные и заблокированные";
                default:
                    throw new ArgumentOutOfRangeException("usersCategory", usersCategory, null);
            }
        }
    }
}