using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Linq;
using RequestsForRights.Database;
using RequestsForRights.Database.Repositories;
using RequestsForRights.Ldap;
using RequestsForRights.Web.Infrastructure.Services;
using RequestsForRights.Web.Models.Models;

namespace SharesMonitoringService
{
    class Program
    {
        static void Main(string[] args)
        {
            var noInLdapUsers = new List<ShareUserInfo>();
            var noInDbUsers = new List<ShareUserInfo>();

            using (var dbContext = new DatabaseContext())
            {
                var resources = dbContext.Resources.Where(r => !r.Deleted && r.Description.ToLower().Contains("shar")
                     && r.Description.ToLower().Contains("(") && r.Description.ToLower().Contains(")"))
                    .ToList()
                    .Select(r => new
                    {
                        r.IdResource,
                        r.Name,
                        r.Description,
                        Share =
                            r.Description.Substring(r.Description.IndexOf('(') + 1,
                                r.Description.IndexOf(')') - r.Description.IndexOf('(') - 1)
                    }).Where(r => !string.IsNullOrEmpty(r.Share));
                var rightService = new RightService(new RightRepository(dbContext));
                var ldapRepository = new LdapRepository(ConfigurationManager.AppSettings["ldap_username"], 
                    ConfigurationManager.AppSettings["ldap_password"]);
                Console.ForegroundColor = ConsoleColor.Green;
                foreach (var resource in resources)
                {
                    Console.WriteLine(@"Processing share {0}", resource.Share);
                    var userResources =
                        rightService.GetResourceRightsOnDate(DateTime.Now.Date, resource.IdResource).GroupBy(r =>
                            new
                            {
                                r.IdRequestUser,
                                r.IdResource,
                                r.ResourceName,
                                r.ResourceDescription
                            }).ToList();
                    var ldapUsers = ldapRepository.GetUsersInGroup(ldapRepository.ConvertGroupNameToCn(resource.Share.Split(',')[0].Trim())).ToList();
                    foreach (var userResource in userResources)
                    {
                        var user = dbContext.Users.FirstOrDefault(r => r.IdRequestUser == userResource.Key.IdRequestUser);
                        if (user != null && user.Login != null && !ldapUsers.Any(u => 
                            string.Equals(u.Login, user.Login, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            Console.WriteLine(@"No user in ldap: {0} ({1}), resource {2}", user.Snp, user.Login, userResource.Key.ResourceName);
                            noInLdapUsers.Add(new ShareUserInfo
                            {
                                Login = user.Login,
                                Snp = user.Snp,
                                ResourceName = userResource.Key.ResourceName,
                                ResourceDescription = userResource.Key.ResourceDescription
                            });
                        }
                    }
                    foreach (var ldapUser in ldapUsers)
                    {
                        var founded = false;
                        foreach (var userResource in userResources)
                        {
                            var user = dbContext.Users.FirstOrDefault(r => r.IdRequestUser == userResource.Key.IdRequestUser);
                            if (user != null && user.Login != null && 
                                string.Equals(ldapUser.Login, user.Login, StringComparison.CurrentCultureIgnoreCase))
                            {
                                founded = true;
                                break;
                            }
                        }
                        if (!founded)
                        {
                            Console.WriteLine(@"No user in rqrights: {0} ({1}), resource {2}", ldapUser.Snp, ldapUser.Login, resource.Name);
                            noInDbUsers.Add(new ShareUserInfo
                            {
                                Login = ldapUser.Login,
                                Snp = ldapUser.Snp,
                                ResourceName = resource.Name,
                                ResourceDescription = resource.Description
                            });
                        }
                    }
                }
            }

            SaveStatisticToFile(ConfigurationManager.AppSettings["shares_statistic_file_name"], noInDbUsers, noInLdapUsers);
        }

        private static void SaveStatisticToFile(string fileName, IEnumerable<ShareUserInfo> noInDbUsers,
            IEnumerable<ShareUserInfo> noInLdapUsers)
        {
            var doc = new XDocument();
            var root = new XElement("shares-rqrights-errors");
            doc.Add(root);
            var noInDbUsersElement = new XElement("no-in-db-users");
            root.Add(noInDbUsersElement);
            foreach (var user in noInDbUsers)
            {
                var userElement = new XElement("user",
                    new XAttribute("Login", user.Login),
                    new XAttribute("Snp", user.Snp),
                    new XAttribute("ResourceName", user.ResourceName),
                    new XAttribute("ResourceDescription", user.ResourceDescription)
                    );
                noInDbUsersElement.Add(userElement);
            }

            var noInLdapUsersElement = new XElement("no-in-ldap-users");
            root.Add(noInLdapUsersElement);
            foreach (var user in noInLdapUsers)
            {
                var userElement = new XElement("user",
                    new XAttribute("Login", user.Login),
                    new XAttribute("Snp", user.Snp),
                    new XAttribute("ResourceName", user.ResourceName),
                    new XAttribute("ResourceDescription", user.ResourceDescription)
                    );
                noInLdapUsersElement.Add(userElement);
            }

            doc.Save(fileName);
        }
    }
}
