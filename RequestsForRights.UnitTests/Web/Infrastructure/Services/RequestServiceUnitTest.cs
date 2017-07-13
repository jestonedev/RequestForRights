using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.UnitTests.Database;
using RequestsForRights.Web.Infrastructure.Enums;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services;
using RequestsForRights.Web.Models.FilterOptions;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ViewModels.Request;

namespace RequestsForRights.UnitTests.Web.Infrastructure.Services
{
    [TestClass]
    public class RequestServiceUnitTest
    {
        [TestMethod]
        public void GetNotSeenRequestsAdmin1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestService = GetRequestService(user, requestsData);
            var requests = requestService.GetNotSeenRequests();
            Assert.AreEqual(12, requests.Count());
        }

        [TestMethod]
        public void GetNotSeenRequestsAdmin2Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 2);
            var requestService = GetRequestService(user, requestsData);
            var requests = requestService.GetNotSeenRequests();
            Assert.AreEqual(13, requests.Count());
        }

        [TestMethod]
        public void DidNotSeenRequests1Admin1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestService = GetRequestService(user, requestsData);
            var notSeen = requestService.DidNotSeenRequest(requestsData.Requests.First(r => r.IdRequest == 1));
            Assert.IsFalse(notSeen);
        }

        [TestMethod]
        public void DidNotSeenRequests2Admin1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestService = GetRequestService(user, requestsData);
            var notSeen = requestService.DidNotSeenRequest(requestsData.Requests.First(r => r.IdRequest == 2));
            Assert.IsTrue(notSeen);
        }

        [TestMethod]
        public void DidNotSeenRequests4Admin1Test()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestService = GetRequestService(user, requestsData);
            var notSeen = requestService.DidNotSeenRequest(requestsData.Requests.First(r => r.IdRequest == 4));
            Assert.IsFalse(notSeen);
        }

        [TestMethod]
        public void GetVisibleRequestsTest()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestService = GetRequestService(user, requestsData);
            var requests = requestService.GetVisibleRequests(
                new RequestsFilterOptions
                {
                    SortDirection = SortDirection.Desc,
                    SortField = "IdRequest",
                    PageSize = 10,
                    PageIndex = 0
                },
                requestsData.Requests.Where(r => !r.Deleted).AsQueryable()
                );
            Assert.AreEqual(10, requests.Count());
            Assert.AreEqual(15, requests.First().IdRequest);
            Assert.AreEqual(6, requests.Last().IdRequest);
        }

        [TestMethod]
        public void GetRequestsCountByStateTypesTest()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestService = GetRequestService(user, requestsData);
            var requestsCount = requestService.GetRequestsCountByStateTypes().ToList();
            Assert.AreEqual(5, requestsCount.Count);
            Assert.AreEqual(2, requestsCount.First(r => r.RequestStateType.IdRequestStateType == 1).RequestCount);
            Assert.AreEqual(3, requestsCount.First(r => r.RequestStateType.IdRequestStateType == 2).RequestCount);
            Assert.AreEqual(2, requestsCount.First(r => r.RequestStateType.IdRequestStateType == 3).RequestCount);
            Assert.AreEqual(0, requestsCount.First(r => r.RequestStateType.IdRequestStateType == 4).RequestCount);
            Assert.AreEqual(5, requestsCount.First(r => r.RequestStateType.IdRequestStateType == 5).RequestCount);
        }

        [TestMethod]
        public void GetRequestByIdTest()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestService = GetRequestService(user, requestsData);
            var request = requestService.GetRequestById(2);
            Assert.AreEqual(2, request.IdRequest);
        }

        [TestMethod]
        public void GetWaitAgreementUsersTest1()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestService = GetRequestService(user, requestsData);
            var waitUsers = requestService.GetWaitAgreementUsers(1, 
                requestsData.RequestAgreements.Where(r => r.IdRequest == 1).ToList()).ToList();
            Assert.AreEqual(2, waitUsers.Count);
            Assert.IsTrue(waitUsers.Any(r => r.IdUser == 7));
            Assert.IsTrue(waitUsers.Any(r => r.IdUser == 9));
        }

        [TestMethod]
        public void GetWaitAgreementUsersTest2()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestService = GetRequestService(user, requestsData);
            var waitUsers = requestService.GetWaitAgreementUsers(11,
                requestsData.RequestAgreements.Where(r => r.IdRequest == 11).ToList()).ToList();
            Assert.AreEqual(0, waitUsers.Count);
        }

        [TestMethod]
        public void GetWaitAgreementUsersTest3()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestService = GetRequestService(user, requestsData);
            var waitUsers = requestService.GetWaitAgreementUsers(15,
                requestsData.RequestAgreements.Where(r => r.IdRequest == 15).ToList()).ToList();
            Assert.AreEqual(1, waitUsers.Count);
        }

        [TestMethod]
        public void GetFilteredRequestsEmptyFilterTest()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestService = GetRequestService(user, requestsData);
            var requests = requestService.GetFilteredRequests(new RequestsFilterOptions());
            Assert.AreEqual(requestsData.Requests.Count(r => !r.Deleted), requests.Count());
        }

        [TestMethod]
        public void GetFilteredRequestsNotSeenTest()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 1);
            var requestService = GetRequestService(user, requestsData);
            var requests = requestService.GetFilteredRequests(new RequestsFilterOptions
            {
                RequestCategory = RequestCategory.NotSeenRequests
            });
            Assert.AreEqual(12, requests.Count());
            Assert.IsFalse(requests.Any(r => r.IdRequest == 1));
            Assert.IsFalse(requests.Any(r => r.IdRequest == 3));
            Assert.IsFalse(requests.Any(r => r.IdRequest == 4));
        }

        [TestMethod]
        public void GetFilteredRequestsMyTest()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 5);
            var requestService = GetRequestService(user, requestsData);
            var requests = requestService.GetFilteredRequests(new RequestsFilterOptions
            {
                RequestCategory = RequestCategory.MyRequests
            });
            Assert.AreEqual(4, requests.Count());
            Assert.IsTrue(requests.Any(r => r.IdRequest == 5));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 10));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 11));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 12));
        }

        [TestMethod]
        public void GetFilteredRequestsDateOfFillingFromTest()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 5);
            var requestService = GetRequestService(user, requestsData);
            var requests = requestService.GetFilteredRequests(new RequestsFilterOptions
            {
                DateOfFillingFrom = new DateTime(2017, 1, 6, 11, 0, 0)
            });
            Assert.AreEqual(4, requests.Count());
            Assert.IsTrue(requests.Any(r => r.IdRequest == 6));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 7));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 8));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 9));
        }

        [TestMethod]
        public void GetFilteredRequestsDateOfFillingToTest()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 5);
            var requestService = GetRequestService(user, requestsData);
            var requests = requestService.GetFilteredRequests(new RequestsFilterOptions
            {
                DateOfFillingTo = new DateTime(2017, 1, 2, 10, 0, 0)
            });
            Assert.AreEqual(3, requests.Count());
            Assert.IsTrue(requests.Any(r => r.IdRequest == 1));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 2));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 15));
        }

        [TestMethod]
        public void GetFilteredRequestsDateOfFillingFromToTest()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 5);
            var requestService = GetRequestService(user, requestsData);
            var requests = requestService.GetFilteredRequests(new RequestsFilterOptions
            {
                DateOfFillingFrom = new DateTime(2017, 1, 7, 0, 0, 0),
                DateOfFillingTo = new DateTime(2017, 1, 8, 23, 59, 59)
            });
            Assert.AreEqual(1, requests.Count());
            Assert.IsTrue(requests.Any(r => r.IdRequest == 7));
        }

        [TestMethod]
        public void GetFilteredRequestsIdRequestStateTypeTest()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 5);
            var requestService = GetRequestService(user, requestsData);
            var requests = requestService.GetFilteredRequests(new RequestsFilterOptions
            {
                IdRequestStateType = 2
            });
            Assert.AreEqual(3, requests.Count());
            Assert.IsTrue(requests.Any(r => r.IdRequest == 9));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 12));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 14));
        }

        [TestMethod]
        public void GetFilteredRequestsIdRequestTypeTest()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 5);
            var requestService = GetRequestService(user, requestsData);
            var requests = requestService.GetFilteredRequests(new RequestsFilterOptions
            {
                IdRequestType = 2
            });
            Assert.AreEqual(5, requests.Count());
            Assert.IsTrue(requests.Any(r => r.IdRequest == 2));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 6));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 10));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 14));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 15));
        }

        [TestMethod]
        public void GetFilteredRequestsFilterTest()
        {
            var requestsData = new DatabaseContext();
            var user = requestsData.AclUsers.First(r => r.IdUser == 5);
            var requestService = GetRequestService(user, requestsData);
            var requests = requestService.GetFilteredRequests(new RequestsFilterOptions
            {
                Filter = "requester1"
            });
            Assert.AreEqual(4, requests.Count());
            Assert.IsTrue(requests.Any(r => r.IdRequest == 1));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 2));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 6));
            Assert.IsTrue(requests.Any(r => r.IdRequest == 7));
        }

        private static RequestService<RequestUserModel, RequestViewModel<RequestUserModel>> GetRequestService(AclUser user, 
            DatabaseContext dbContext)
        {
            var requestRepositoryMock = new Mock<IRequestRepository>();
            requestRepositoryMock.Setup(r => r.GetRequests())
                .Returns(dbContext.Requests.Where(r => !r.Deleted).AsQueryable());
            requestRepositoryMock.Setup(r => r.GetRequestsUserLastSeens(user.Login))
                .Returns(dbContext.RequestUserLastSeens.Where(r => r.User.Login.ToLower() == user.Login.ToLower())
                .AsQueryable());
            requestRepositoryMock.Setup(r => r.GetRequestStateTypes())
                .Returns(dbContext.RequestStateTypes.AsQueryable());
            var resourceRepositoryMock = new Mock<IResourceRepository>();
            foreach (var request in dbContext.Requests)
            {
                var req = request;
                requestRepositoryMock.Setup(r => r.GetRequestById(req.IdRequest, false))
                .Returns(request);
            }
            var requestSecurityService = new Mock<IRequestSecurityService<RequestUserModel>>();
            requestSecurityService.Setup(r => r.FilterRequests(It.IsAny<IQueryable<Request>>()))
                .Returns(dbContext.Requests.Where(r => !r.Deleted).AsQueryable());
            requestSecurityService.SetupGet(r => r.CurrentUser).Returns(user.Login);
            foreach (var aclUser in dbContext.AclUsers)
            {
                var u = aclUser;
                requestSecurityService.Setup(r => r.GetUserAllowedDepartments(u)).Returns(() =>
                    u.AclDepartments.Any()
                        ? u.AclDepartments.AsQueryable()
                        : new List<Department> {u.Department}.AsQueryable());
            }
            return new RequestService<RequestUserModel, RequestViewModel<RequestUserModel>>(
                requestRepositoryMock.Object, resourceRepositoryMock.Object,  requestSecurityService.Object);
        }
    }
}
