using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Linq;
using RequestsForRights.Domain.Enums;

namespace RequestsForRights.Ldap
{
    public sealed class LdapRepository: ILdapRepository
    {
        private readonly string _userName;
        private readonly string _password;

        private readonly string[] _propertiesToLoad = {

            "displayName",
            "samAccountName",
            "title",
            "company",
            "department",
            "physicaldeliveryofficename",
            "mail",
            "telephonenumber"
        };

        public LdapRepository(string userName, string password)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }
            _userName = userName;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            _password = password;
        }
        private static IEnumerable<string> GetDomains()
        {
            return from System.DirectoryServices.ActiveDirectory.Domain domain in Forest.GetCurrentForest().Domains 
                   select domain.Name;
        }

        public IEnumerable<LdapUser> FindUsers(string snpPattern, UsersCategory usersCategory,
            IEnumerable<LdapDepartmentFilter> departemtnsFilter, int maxCount)
        {
            if (string.IsNullOrEmpty(snpPattern))
            {
                throw new ArgumentNullException("snpPattern");
            }
            var i = 0;
            var users = new List<LdapUser>();
            var filters = departemtnsFilter.ToList();
            var extFilter = "";
            switch (usersCategory)
            {
                case UsersCategory.ActiveUsers:
                    extFilter = "(!(useraccountcontrol:1.2.840.113556.1.4.803:=2))";
                    break;
                case UsersCategory.BlockedUsers:
                    extFilter = "((useraccountcontrol:1.2.840.113556.1.4.803:=2))";
                    break;
                case UsersCategory.All:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("usersCategory", usersCategory, null);
            }
            foreach (var domainName in GetDomains())
            {
                var context = new DirectoryContext(DirectoryContextType.Domain, domainName, _userName, _password);
                using (var domain = System.DirectoryServices.ActiveDirectory.Domain.GetDomain(context))
                using (var domainEntry = domain.GetDirectoryEntry())
                {
                    var domainLoginPrefix = domain.Name.Split('.')[0];
                    using (var searcher = new DirectorySearcher(domainEntry, "", _propertiesToLoad))
                    {
                        searcher.SearchScope = SearchScope.Subtree;
                        foreach (var filter in filters)
                        {
                            searcher.Filter = string.Format(CultureInfo.InvariantCulture, 
                                "(&(objectClass=user)(objectClass=person){1}{2}(displayName=*{0}*){3})", 
                                snpPattern, 
                                string.IsNullOrEmpty(filter.Company) ? "" : string.Format("(company={0})", filter.Company), 
                                string.IsNullOrEmpty(filter.Department) ? "" : string.Format("(department={0})", filter.Department),
                                extFilter);
                            var results = searcher.FindAll();
                            if (results.Count == 0)
                                continue;
                            foreach (SearchResult result in results)
                            {
                                var user = SearchResultToUser(result, domainLoginPrefix);
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

        public IEnumerable<LdapUser> FindMaternityLeaveUsers(string snpPattern,
            IEnumerable<LdapDepartmentFilter> departemtnsFilter, int maxCount)
        {
            if (string.IsNullOrEmpty(snpPattern))
            {
                throw new ArgumentNullException("snpPattern");
            }
            var i = 0;
            var users = new List<LdapUser>();
            var filters = departemtnsFilter.ToList();
            using (var domainEntry = new DirectoryEntry("LDAP://OU=dekrete,OU=_DisableUser,DC=pwr,DC=mcs,DC=br", _userName, _password))
            {
                using (var searcher = new DirectorySearcher(domainEntry, "", _propertiesToLoad))
                {
                    searcher.SearchScope = SearchScope.Subtree;
                    foreach (var filter in filters)
                    {
                        searcher.Filter = string.Format(CultureInfo.InvariantCulture,
                            "(&(objectClass=user)(objectClass=person){1}{2}(displayName=*{0}*))",
                            snpPattern,
                            string.IsNullOrEmpty(filter.Company) ? "" : string.Format("(company={0})", filter.Company),
                            string.IsNullOrEmpty(filter.Department) ? "" : string.Format("(department={0})", filter.Department));
                        var results = searcher.FindAll();
                        if (results.Count == 0)
                            continue;
                        foreach (SearchResult result in results)
                        {
                            var user = SearchResultToUser(result, "pwr");
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
                        return users;
                    }
                }
            }
            return users;
        }

        private LdapUser SearchResultToUser(SearchResult result, string domainLoginPrefix)
        {
            return new LdapUser
            {
                Snp = GetValue(result.Properties, "displayName"),
                Login = domainLoginPrefix.ToLower() + "\\" + GetValue(result.Properties, "samAccountName").ToLower(),
                Post = GetValue(result.Properties, "title"),
                Department = GetValue(result.Properties, "company"),
                Unit = GetValue(result.Properties, "department"),
                Office = GetValue(result.Properties, "physicaldeliveryofficename"),
                Email = GetValue(result.Properties, "mail"),
                Phone = GetValue(result.Properties, "telephonenumber")
            };
        }

        private static string GetValue(ResultPropertyCollection properties, string parameter)
        {
            return properties.Contains(parameter) ? properties[parameter][0].ToString() : null;
        }
    }
}
