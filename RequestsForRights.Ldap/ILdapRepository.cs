using System.Collections.Generic;
using RequestsForRights.Domain.Enums;

namespace RequestsForRights.Ldap
{
    public interface ILdapRepository
    {
        IEnumerable<LdapUser> FindUsers(string snpPattern, UsersCategory usersCategory,
            IEnumerable<LdapDepartmentFilter> departemtnsFilter, int maxCount);

        IEnumerable<LdapUser> FindMaternityLeaveUsers(string snpPattern,
            IEnumerable<LdapDepartmentFilter> departemtnsFilter, int maxCount);
    }
}
