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

        public IEnumerable<LdapUser> FindUsers(string snpPattern, 
            IEnumerable<LdapDepartmentFilter> departemtnsFilter, int maxCount)
        {
            if (string.IsNullOrEmpty(snpPattern))
            {
                throw new ArgumentNullException("snpPattern");
            }
            var i = 0;
            var users = new List<LdapUser>();
            var filters = departemtnsFilter.ToList();
            foreach (var domainName in GetDomains())
            {
                var context = new DirectoryContext(
                    DirectoryContextType.Domain, domainName, _userName, _password);
                using (var domain = Domain.GetDomain(context))
                using (var domainEntry = domain.GetDirectoryEntry())
                {
                    using (var searcher = new DirectorySearcher())
                    {
                        searcher.SearchRoot = domainEntry;
                        searcher.SearchScope = SearchScope.Subtree;
                        searcher.PropertiesToLoad.Add("displayName");
                        searcher.PropertiesToLoad.Add("samAccountName");
                        searcher.PropertiesToLoad.Add("title");
                        searcher.PropertiesToLoad.Add("company");
                        searcher.PropertiesToLoad.Add("department");
                        searcher.PropertiesToLoad.Add("physicaldeliveryofficename");
                        searcher.PropertiesToLoad.Add("mail");
                        searcher.PropertiesToLoad.Add("telephonenumber");
                        foreach (var filter in filters)
                        {
                            searcher.Filter = string.Format(CultureInfo.InvariantCulture,
                            "(&(objectClass=user)(objectClass=person){1}{2}(displayName=*{0}*)(!(useraccountcontrol:1.2.840.113556.1.4.803:=2)))",
                            snpPattern,
                                filter.Company == null ? "" : string.Format("(company={0})", filter.Company),
                                filter.Department == null ? "" : string.Format("(department={0})", filter.Department));
                            var results = searcher.FindAll();
                            if (results.Count == 0)
                                continue;
                            foreach (SearchResult result in results)
                            {
                                var user = new LdapUser
                                {
                                    Snp = GetValue(result.Properties, "displayName"),
                                    Login = GetValue(result.Properties, "samAccountName").ToLower(),
                                    Post = GetValue(result.Properties, "title"),
                                    Department = GetValue(result.Properties, "company"),
                                    Unit = GetValue(result.Properties, "department"),
                                    Office = GetValue(result.Properties, "physicaldeliveryofficename"),
                                    Email = GetValue(result.Properties, "mail"),
                                    Phone = GetValue(result.Properties, "telephonenumber")
                                };
                                users.Add(user);
                                i++;
                                if (i == maxCount)
                                {
                                    break;
                                }
                            }
                            if (i == maxCount)
                            {
                                break;
                            }
                        }
                        if (i == maxCount)
                        {
                            break;
                        }
                    }
                }
            }
            return users;
        }

        private static string GetValue(ResultPropertyCollection properties, string parameter)
        {
            return properties.Contains(parameter)
                ? properties[parameter][0].ToString()
                : null;
        }
    }
}
