using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.UnitTests.Database;
using RequestsForRights.Web.Infrastructure.Security;
using RequestsForRights.Web.Models.Models;

namespace RequestsForRights.UnitTests.Web.Infrastructure.Security
{
    [TestClass]
    public class RequestSecurityServiceUnitTest
    {
        [TestMethod]
        public void FilterRequestsAdmin1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var requests = requestSecurityService.FilterRequests(requestsData.Requests.AsQueryable());
            Assert.AreEqual(14, requests.Count());
        }

        [TestMethod]
        public void FilterRequestsRequester1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 3);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var requests = requestSecurityService.FilterRequests(requestsData.Requests.AsQueryable()).ToList();
            Assert.AreEqual(9, requests.Count);
            Assert.IsTrue(requests.Any(r => r.IdRequest == 1));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 2));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 3));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 6));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 7));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 8));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 9));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 13));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 14));
        }

        [TestMethod]
        public void FilterRequestsRequester2Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 4);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var requests = requestSecurityService.FilterRequests(requestsData.Requests.AsQueryable());
            Assert.AreEqual(14, requests.Count());
        }

        [TestMethod]
        public void FilterRequestsRequester3Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 5);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var requests = requestSecurityService.FilterRequests(requestsData.Requests.AsQueryable());
            Assert.AreEqual(9, requests.Count());
            Assert.IsTrue(requests.Any(r => r.IdRequest == 5));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 10));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 11));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 12));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 3));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 8));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 9));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 13));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 14));
        }

        [TestMethod]
        public void FilterRequestsResourceOwner1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 6);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var requests = requestSecurityService.FilterRequests(requestsData.Requests.AsQueryable());
            Assert.AreEqual(10, requests.Count());
            Assert.IsTrue(requests.Any(r => r.IdRequest == 2));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 5));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 6));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 7));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 9));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 10));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 11));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 13));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 14));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 15));
        }

        [TestMethod]
        public void FilterRequestsResourceOwner2Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 7);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var requests = requestSecurityService.FilterRequests(requestsData.Requests.AsQueryable());
            Assert.AreEqual(8, requests.Count());
            Assert.IsTrue(requests.Any(r => r.IdRequest == 1));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 3));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 6));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 9));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 10));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 11));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 14));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 15));
        }

        [TestMethod]
        public void FilterRequestsResourceOwner3Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 8);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var requests = requestSecurityService.FilterRequests(requestsData.Requests.AsQueryable());
            Assert.AreEqual(10, requests.Count());
            Assert.IsTrue(requests.Any(r => r.IdRequest == 2));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 5));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 6));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 7));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 9));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 10));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 11));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 13));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 14));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 15));
        }

        [TestMethod]
        public void FilterRequestsCoordinator1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 9);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var requests = requestSecurityService.FilterRequests(requestsData.Requests.AsQueryable());
            Assert.AreEqual(4, requests.Count());
            Assert.IsTrue(requests.Any(r => r.IdRequest == 1));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 8));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 10));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 11));
        }

        [TestMethod]
        public void FilterRequestsCoordinator2Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 10);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var requests = requestSecurityService.FilterRequests(requestsData.Requests.AsQueryable());
            Assert.AreEqual(3, requests.Count());
            Assert.IsTrue(requests.Any(r => r.IdRequest == 9));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 10));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 11));
        }

        [TestMethod]
        public void FilterRequestsDispatcher1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 11);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var requests = requestSecurityService.FilterRequests(requestsData.Requests.AsQueryable());
            Assert.AreEqual(10, requests.Count());
            Assert.IsTrue(requests.Any(r => r.IdRequest == 2));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 3));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 7));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 8));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 9));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 10));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 11));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 12));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 13));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 14));
        }

        [TestMethod]
        public void FilterRequestsRegistrar1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 12);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var requests = requestSecurityService.FilterRequests(requestsData.Requests.AsQueryable());
            Assert.AreEqual(10, requests.Count());
            Assert.IsTrue(requests.Any(r => r.IdRequest == 2));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 3));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 7));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 8));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 9));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 10));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 11));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 12));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 13));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 14));
        }

        [TestMethod]
        public void FilterRequestsExecutor1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 13);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var requests = requestSecurityService.FilterRequests(requestsData.Requests.AsQueryable());
            Assert.AreEqual(4, requests.Count());
            Assert.IsTrue(requests.Any(r => r.IdRequest == 2));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 3));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 10));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 13));
        }

        [TestMethod]
        public void FilterRequestsResourceManager1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 14);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var requests = requestSecurityService.FilterRequests(requestsData.Requests.AsQueryable());
            Assert.AreEqual(0, requests.Count());
        }

        [TestMethod]
        public void CanSetRequest1StateGlobalAdmin2Test1()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 2);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestStateGlobal(
                requestsData.Requests.First(r => r.IdRequest == 1), 1);
            Assert.IsTrue(can);
        }

        [TestMethod]
        public void CanSetRequest1StateGlobalAdmin2Test2()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 2);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestStateGlobal(
                requestsData.Requests.First(r => r.IdRequest == 1), 2);
            Assert.IsTrue(can);
        }

        [TestMethod]
        public void CanSetRequest2StateGlobalAdmin2Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 2);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestStateGlobal(
                requestsData.Requests.First(r => r.IdRequest == 2), 1);
            Assert.IsFalse(can);
        }

        [TestMethod]
        public void CanSetRequest12StateGlobalAdmin2Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 2);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestStateGlobal(
                requestsData.Requests.First(r => r.IdRequest == 12), 1);
            Assert.IsFalse(can);
        }

        [TestMethod]
        public void CanSetRequest4StateGlobalAdmin2Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 2);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestStateGlobal(
                requestsData.Requests.First(r => r.IdRequest == 4), 1);
            Assert.IsFalse(can);
        }

        [TestMethod]
        public void CanSetRequest1State1Admin1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 1), 1);
            Assert.IsFalse(can);
        }

        [TestMethod]
        public void CanSetRequest3State1Admin1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 3), 1);
            Assert.IsTrue(can);
        }

        [TestMethod]
        public void CanSetRequest1State2Admin1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 1), 2);
            Assert.IsTrue(can);
        }

        [TestMethod]
        public void CanSetRequest1State3Admin1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 1), 3);
            Assert.IsTrue(can);
        }

        [TestMethod]
        public void CanSetRequest1State4Admin1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 1), 4);
            Assert.IsTrue(can);
        }

        [TestMethod]
        public void CanSetRequest1State5Admin1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 1), 5);
            Assert.IsTrue(can);
        }

        [TestMethod]
        public void CanSetRequest1State2ResourceOwner1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 6);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 1), 2);
            Assert.IsFalse(can);
        }

        [TestMethod]
        public void CanSetRequest1State2ResourceOwner2Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 7);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 1), 2);
            Assert.IsTrue(can);
        }

        [TestMethod]
        public void CanSetRequest1State2ResourceOwner3Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 8);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 1), 2);
            Assert.IsFalse(can);
        }

        [TestMethod]
        public void CanSetRequest6State2ResourceOwner1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 6);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 6), 2);
            Assert.IsFalse(can);
        }

        [TestMethod]
        public void CanSetRequest6State2ResourceOwner2Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 7);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 6), 2);
            Assert.IsFalse(can);
        }

        [TestMethod]
        public void CanSetRequest1State5ResourceOwner2Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 7);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 1), 5);
            Assert.IsTrue(can);
        }

        [TestMethod]
        public void CanSetRequest15State2ResourceOwner3Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 7);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 15), 2);
            Assert.IsTrue(can);
        }

        [TestMethod]
        public void CanSetRequest1State2Dispatcher1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 11);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 1), 2);
            Assert.IsFalse(can);
        }

        [TestMethod]
        public void CanSetRequest1State5Dispatcher1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 11);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 1), 5);
            Assert.IsFalse(can);
        }

        [TestMethod]
        public void CanSetRequest12State5Dispatcher1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 11);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 12), 5);
            Assert.IsTrue(can);
        }

        [TestMethod]
        public void CanSetRequest9State3Dispatcher1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 11);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 9), 3);
            Assert.IsTrue(can);
        }

        [TestMethod]
        public void CanSetRequest10State3Dispatcher1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 11);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 10), 3);
            Assert.IsFalse(can);
        }

        [TestMethod]
        public void CanSetRequest1State3Dispatcher1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 11);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 1), 3);
            Assert.IsFalse(can);
        }

        [TestMethod]
        public void CanSetRequest8State2Coordinator1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 9);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 8), 2);
            Assert.IsTrue(can);
        }

        [TestMethod]
        public void CanSetRequest8State5Coordinator1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 9);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 8), 5);
            Assert.IsTrue(can);
        }

        [TestMethod]
        public void CanSetRequest1State4Coordinator1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 9);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 1), 4);
            Assert.IsFalse(can);
        }

        [TestMethod]
        public void CanSetRequest11State5Coordinator2Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 10);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 11), 5);
            Assert.IsFalse(can);
        }

        [TestMethod]
        public void CanSetRequest1State10Admin1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 1), 10);
            Assert.IsFalse(can);
        }

        [TestMethod]
        public void CanSetRequest1State1Requester1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 3);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState(
                requestsData.Requests.First(r => r.IdRequest == 1), 1);
            Assert.IsFalse(can);
        }

        [TestMethod]
        public void CanSetNullRequestStateTest()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 3);
            var requestSecurityService = GetRequestSecurityService(user, requestsData);
            var can = requestSecurityService.CanSetRequestState((Request)null, 1);
            Assert.IsFalse(can);
        }

        private static RequestSecurityService<RequestUserModel> GetRequestSecurityService(AclUser user, DatabaseContext dbContext)
        {
            var securityRepositoryMock = new Mock<ISecurityRepository>();
            securityRepositoryMock.Setup(r => r.GetUserInfo(user.Login)).Returns(user);
            securityRepositoryMock.Setup(r => r.GetUserInfo(null)).Returns(user);
            foreach (var aclUser in dbContext.AclUsers)
            {
                var u = aclUser;
                securityRepositoryMock.Setup(r => r.GetUserAllowedDepartments(u.Login)).Returns(() =>
                u.AclDepartments.Any()
                    ? u.AclDepartments.AsQueryable()
                    : new List<Department> { u.Department }.AsQueryable());
            }
            securityRepositoryMock.Setup(r => r.GetUserRoles(null)).Returns(() => user.Roles.AsQueryable());
            var requestRepositoryMock = new Mock<IRequestRepository>();
            foreach (var request in dbContext.Requests)
            {
                var rq = request;
                requestRepositoryMock.Setup(r => r.GetRequestById(rq.IdRequest, false))
                    .Returns(dbContext.Requests.First(r => r.IdRequest == rq.IdRequest));
            }
            var resourceRepositoryMock = new Mock<IResourceRepository>();
            resourceRepositoryMock.Setup(r => r.GetResourceRights()).Returns(
                () => dbContext.ResourceRights.AsQueryable());
            return new RequestSecurityService<RequestUserModel>(securityRepositoryMock.Object,
                requestRepositoryMock.Object, resourceRepositoryMock.Object);
        }
    }
}
