using System.Collections.Generic;

namespace RequestsForRights.Ldap
{
    public interface ILdapRepository
    {
        IEnumerable<LdapUser> FindUsers(string snpPattern, 
            IEnumerable<LdapDepartmentFilter> departemtnsFilter, int maxCount);
    }
}
