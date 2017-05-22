using System;
using RequestsForRights.Domain.Enums;

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
                case UsersCategory.All:
                    return "Все";
                default:
                    throw new ArgumentOutOfRangeException("usersCategory", usersCategory, null);
            }
        }
    }
}