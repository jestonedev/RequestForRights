using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Linq;

namespace RequestsForRights.Ldap
{
    public sealed class LdapRepository: ILdapRepository
    {
        private readonly string _userName;
        private readonly string _password;

        public LdapRepository(string userName, string password)
        {
            _userName = userName;
            _password = password;
        }
        private static IEnumerable<string> GetDomains()
        {
            return from Domain domain in Forest.GetCurrentForest().Domains 
                   select domain.Name;
        }

        public LdapUser GetUserInfo(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentNullException("login");
            }
            foreach (var domainName in GetDomains())
            {
                var context = new DirectoryContext(
                    DirectoryContextType.Domain, domainName, _userName, _password);
                using(var domain = Domain.GetDomain(context))
                using (var domainEntry = domain.GetDirectoryEntry())
                {
                    using (var searcher = new DirectorySearcher())
                    {
                        searcher.SearchRoot = domainEntry;
                        searcher.SearchScope = SearchScope.Subtree;
                        searcher.PropertiesToLoad.Add("displayName");
                        searcher.PropertiesToLoad.Add("samAccountName");
                        searcher.PropertiesToLoad.Add("company");
                        searcher.PropertiesToLoad.Add("department");
                        searcher.PropertiesToLoad.Add("office");
                        searcher.PropertiesToLoad.Add("mail");
                        searcher.PropertiesToLoad.Add("phone");
                        var loginParts = login.Split('\\');
                        searcher.Filter = string.Format(CultureInfo.InvariantCulture,
                            "(&(objectClass=user)(objectClass=person)(samAccountName={0})(!(useraccountcontrol:1.2.840.113556.1.4.803:=2)))", 
                            loginParts[loginParts.Length - 1]);
                        var results = searcher.FindAll();
                        if (results.Count == 0)
                            continue;
                        var userDomain = new LdapUser
                        {
                            DisplayName = results[0].Properties["displayName"][0].ToString(),
                            Login = results[0].Properties["samAccountName"][0].ToString().ToLower(),
                            Company = results[0].Properties["company"][0].ToString(),
                            Department = results[0].Properties["department"][0].ToString(),
                            Office = results[0].Properties["office"][0].ToString(),
                            Email = results[0].Properties["mail"][0].ToString(),
                            Phone = results[0].Properties["phone"][0].ToString(),
                        };
                        return userDomain;
                    }
                }
            }
            return null;
        }
    }
}
