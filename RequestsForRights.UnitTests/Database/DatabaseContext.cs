using System;
using System.Collections.Generic;
using System.Linq;
using RequestsForRights.Domain.Entities;

namespace RequestsForRights.UnitTests.Database
{
    internal class DatabaseContext
    {
        public List<ResourceRight> ResourceRights { get; set; }
        public List<Resource> Resources { get; set; }
        public List<ResourceGroup> ResourceGroups { get; set; }
        public List<Department> Departments { get; set; }
        public List<AclUser> AclUsers { get; set; }
        public List<AclRole> AclRoles { get; set; }
        public List<Request> Requests { get; set; }
        public List<RequestType> RequestTypes { get; set; }
        public List<RequestStateType> RequestStateTypes { get; set; }
        public List<RequestUser> Users { get; set; }
        public List<RequestUserAssoc> RequestUserAssocs { get; set; }
        public List<RequestUserRightAssoc> RequestUserRightAssocs { get; set; }
        public List<DelegationRequestUsersExtInfo> DelegationRequestUsersExtInfo { get; set; }
        public List<RequestExtComment> RequestExtComments { get; set; }
        public List<RequestAgreementType> RequestAgreementTypes { get; set; }
        public List<RequestAgreementState> RequestAgreementStates { get; set; }
        public List<RequestAgreement> RequestAgreements { get; set; }
        public List<RequestUserLastSeen> RequestUserLastSeens { get; set; }
        public List<RequestState> RequestStates { get; set; }
        public List<RequestRightGrantType> RequestRightGrantTypes { get; set; }
        public List<ResourceInformationType> ResourceInformationTypes { get; set; }
        public List<ResourceDeviceAddress> ResourceDeviceAddresses { get; set; }
        public List<ResourceInternetAddress> ResourceInternetAddresses { get; set; }
        public List<ResourceOperatorPerson> ResourceOperatorPersons { get; set; }
        public List<ResourceOperatorPersonAct> ResourceOperatorPersonActs { get; set; }
        public List<ResourceOwnerPerson> ResourceOwnerPersons { get; set; }
        public List<ResourceOwnerPersonAct> ResourceOwnerPersonActs { get; set; }
        public List<ResourceAuthorityAct> ResourceAuthorityActs { get; set; }
        public List<ResourceOperatorAct> ResourceOperatorActs { get; set; }
        public List<ResourceUsingAct> ResourceUsingActs { get; set; }
        public List<ActFile> ActFiles { get; set; }

        public DatabaseContext()
        {
            InitializeCollections();
            LoadData();
        }

        private void LoadData()
        {
            LoadAclRoles();
            LoadDepartments();
            LoadUsers();
            LoadRequestStateTypes();
            LoadRequestTypes();
            LoadRequests();
            LoadRequestStates();
            LoadRequestUsers();
            LoadRequestUserAssocs();
            LoadResourceGroups();
            LoadResources();
            LoadResourceRights();
        }

        private void LoadResourceRights()
        {
            throw new NotImplementedException();
        }

        private void LoadResources()
        {
            throw new NotImplementedException();
        }

        private void LoadResourceGroups()
        {
            throw new NotImplementedException();
        }

        private void LoadAclRoles()
        {
            AclRoles.Add(new AclRole { IdRole = 1, Users = new List<AclUser>() });
            AclRoles.Add(new AclRole { IdRole = 2, Users = new List<AclUser>() });
            AclRoles.Add(new AclRole { IdRole = 3, Users = new List<AclUser>() });
            AclRoles.Add(new AclRole { IdRole = 4, Users = new List<AclUser>() });
            AclRoles.Add(new AclRole { IdRole = 5, Users = new List<AclUser>() });
            AclRoles.Add(new AclRole { IdRole = 6, Users = new List<AclUser>() });
            AclRoles.Add(new AclRole { IdRole = 7, Users = new List<AclUser>() });
            AclRoles.Add(new AclRole { IdRole = 8, Users = new List<AclUser>() });
        }

        private void LoadDepartments()
        {
            var department1 = new Department
            {
                IdDepartment = 1,
                AclUsers = new List<AclUser>(),
                ChildDepartments = new List<Department>(),
                Users = new List<AclUser>()
            };
            Departments.Add(department1);
            var department11 = new Department
            {
                IdDepartment = 11,
                IdParentDepartment = 1,
                ParentDepartment = department1,
                AclUsers = new List<AclUser>(),
                ChildDepartments = new List<Department>(),
                Users = new List<AclUser>()
            };
            Departments.Add(department11);
            department1.ChildDepartments.Add(department11);
            var department12 = new Department
            {
                IdDepartment = 12,
                IdParentDepartment = 1,
                ParentDepartment = department1,
                AclUsers = new List<AclUser>(),
                ChildDepartments = new List<Department>(),
                Users = new List<AclUser>()
            };
            Departments.Add(department12);
            department1.ChildDepartments.Add(department12);
            var department2 = new Department
            {
                IdDepartment = 2,
                AclUsers = new List<AclUser>(),
                ChildDepartments = new List<Department>(),
                Users = new List<AclUser>()
            };
            Departments.Add(department2);
            var department21 = new Department
            {
                IdDepartment = 21,
                IdParentDepartment = 2,
                ParentDepartment = department2,
                AclUsers = new List<AclUser>(),
                ChildDepartments = new List<Department>(),
                Users = new List<AclUser>()
            };
            Departments.Add(department21);
            department2.ChildDepartments.Add(department21);
            var department22 = new Department
            {
                IdDepartment = 22,
                IdParentDepartment = 2,
                ParentDepartment = department2,
                AclUsers = new List<AclUser>(),
                ChildDepartments = new List<Department>(),
                Users = new List<AclUser>()
            };
            Departments.Add(department22);
            department2.ChildDepartments.Add(department22);
            var department3 = new Department
            {
                IdDepartment = 3,
                AclUsers = new List<AclUser>(),
                ChildDepartments = new List<Department>(),
                Users = new List<AclUser>()
            };
            Departments.Add(department3);
            var department31 = new Department
            {
                IdDepartment = 31,
                IdParentDepartment = 3,
                ParentDepartment = department3,
                AclUsers = new List<AclUser>(),
                ChildDepartments = new List<Department>(),
                Users = new List<AclUser>()
            };
            Departments.Add(department31);
            department3.ChildDepartments.Add(department31);
            var department4 = new Department
            {
                IdDepartment = 24, // CIT
                AclUsers = new List<AclUser>(),
                ChildDepartments = new List<Department>(),
                Users = new List<AclUser>()
            };
            Departments.Add(department4);
        }

        private void LoadUsers()
        {
            // Administrators
            var userAdministrator1 = new AclUser
            {
                IdUser = 1,
                Login = "admin1",
                Roles = new List<AclRole> { AclRoles.First(r => r.IdRole == 1) },
                IdDepartment = 24,
                Department = Departments.First(r => r.IdDepartment == 24),
                AclDepartments = new List<Department>(),
                Requests = new List<Request>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestsExtComments = new List<RequestExtComment>()
            };
            Departments.First(r => r.IdDepartment == 24).Users.Add(userAdministrator1);
            AclRoles.First(r => r.IdRole == 1).Users.Add(userAdministrator1);
            AclUsers.Add(userAdministrator1);
            var userAdministrator2 = new AclUser
            {
                IdUser = 2,
                Login = "admin2",
                Roles = new List<AclRole> { AclRoles.First(r => r.IdRole == 1) },
                IdDepartment = 2,
                Department = Departments.First(r => r.IdDepartment == 2),
                AclDepartments = new List<Department>(),
                Requests = new List<Request>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestsExtComments = new List<RequestExtComment>()
            };
            Departments.First(r => r.IdDepartment == 2).Users.Add(userAdministrator2);
            AclRoles.First(r => r.IdRole == 1).Users.Add(userAdministrator2);
            AclUsers.Add(userAdministrator2);

            // Requesters
            var userRequester1 = new AclUser
            {
                IdUser = 3,
                Login = "requester1",
                Roles = new List<AclRole> { AclRoles.First(r => r.IdRole == 3) },
                IdDepartment = 2,
                Department = Departments.First(r => r.IdDepartment == 2),
                AclDepartments = new List<Department>(),
                Requests = new List<Request>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestsExtComments = new List<RequestExtComment>()
            };
            Departments.First(r => r.IdDepartment == 2).Users.Add(userRequester1);
            AclRoles.First(r => r.IdRole == 3).Users.Add(userRequester1);
            AclUsers.Add(userRequester1);
            var userRequester2 = new AclUser
            {
                IdUser = 4,
                Login = "requester2",
                Roles = new List<AclRole>
                {
                    AclRoles.First(r => r.IdRole == 3), 
                    AclRoles.First(r => r.IdRole == 2)
                },
                IdDepartment = 3,
                Department = Departments.First(r => r.IdDepartment == 3),
                AclDepartments = new List<Department>
                {
                    Departments.First(r => r.IdDepartment == 1),
                    Departments.First(r => r.IdDepartment == 2)
                },
                Requests = new List<Request>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestsExtComments = new List<RequestExtComment>()
            };
            Departments.First(r => r.IdDepartment == 3).Users.Add(userRequester2);
            AclRoles.First(r => r.IdRole == 3).Users.Add(userRequester2);
            AclRoles.First(r => r.IdRole == 2).Users.Add(userRequester2);
            AclUsers.Add(userRequester2);
            var userRequester3 = new AclUser
            {
                IdUser = 5,
                Login = "requester3",
                Roles = new List<AclRole>
                {
                    AclRoles.First(r => r.IdRole == 3), 
                    AclRoles.First(r => r.IdRole == 2), 
                    AclRoles.First(r => r.IdRole == 8)
                },
                IdDepartment = 1,
                Department = Departments.First(r => r.IdDepartment == 1),
                AclDepartments = new List<Department>(),
                Requests = new List<Request>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestsExtComments = new List<RequestExtComment>()
            };
            Departments.First(r => r.IdDepartment == 1).Users.Add(userRequester3);
            AclRoles.First(r => r.IdRole == 3).Users.Add(userRequester3);
            AclRoles.First(r => r.IdRole == 2).Users.Add(userRequester3);
            AclRoles.First(r => r.IdRole == 8).Users.Add(userRequester3);
            AclUsers.Add(userRequester3);

            // Resource Owners
            var userResourceOwner1 = new AclUser
            {
                IdUser = 6,
                Login = "resourceOwner1",
                Roles = new List<AclRole> { AclRoles.First(r => r.IdRole == 2) },
                IdDepartment = 2,
                Department = Departments.First(r => r.IdDepartment == 2),
                AclDepartments = new List<Department>(),
                Requests = new List<Request>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestsExtComments = new List<RequestExtComment>()
            };
            Departments.First(r => r.IdDepartment == 2).Users.Add(userResourceOwner1);
            AclRoles.First(r => r.IdRole == 2).Users.Add(userResourceOwner1);
            AclUsers.Add(userResourceOwner1);
            var userResourceOwner2 = new AclUser
            {
                IdUser = 7,
                Login = "resourceOwner2",
                Roles = new List<AclRole>
                {
                    AclRoles.First(r => r.IdRole == 2), 
                    AclRoles.First(r => r.IdRole == 8)
                },
                IdDepartment = 3,
                Department = Departments.First(r => r.IdDepartment == 3),
                AclDepartments = new List<Department>(),
                Requests = new List<Request>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestsExtComments = new List<RequestExtComment>()
            };
            Departments.First(r => r.IdDepartment == 3).Users.Add(userResourceOwner2);
            AclRoles.First(r => r.IdRole == 2).Users.Add(userResourceOwner2);
            AclRoles.First(r => r.IdRole == 8).Users.Add(userResourceOwner2);
            AclUsers.Add(userResourceOwner2);
            var userResourceOwner3 = new AclUser
            {
                IdUser = 8,
                Login = "resourceOwner3",
                Roles = new List<AclRole>
                {
                    AclRoles.First(r => r.IdRole == 2)
                },
                IdDepartment = 3,
                Department = Departments.First(r => r.IdDepartment == 3),
                AclDepartments = new List<Department>
                {
                    Departments.First(r => r.IdDepartment == 2),
                    Departments.First(r => r.IdDepartment == 1)
                },
                Requests = new List<Request>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestsExtComments = new List<RequestExtComment>()
            };
            Departments.First(r => r.IdDepartment == 3).Users.Add(userResourceOwner3);
            Departments.First(r => r.IdDepartment == 2).AclUsers.Add(userResourceOwner3);
            Departments.First(r => r.IdDepartment == 1).AclUsers.Add(userResourceOwner3);
            AclRoles.First(r => r.IdRole == 2).Users.Add(userResourceOwner3);
            AclRoles.First(r => r.IdRole == 8).Users.Add(userResourceOwner3);
            AclUsers.Add(userResourceOwner2);

            // Coordinators
            var userCoordinator1 = new AclUser
            {
                IdUser = 8,
                Login = "coordinator1",
                Roles = new List<AclRole> { AclRoles.First(r => r.IdRole == 8) },
                IdDepartment = 1,
                Department = Departments.First(r => r.IdDepartment == 1),
                AclDepartments = new List<Department>(),
                Requests = new List<Request>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestsExtComments = new List<RequestExtComment>()
            };
            Departments.First(r => r.IdDepartment == 1).Users.Add(userCoordinator1);
            AclRoles.First(r => r.IdRole == 8).Users.Add(userCoordinator1);
            AclUsers.Add(userCoordinator1);
            var userCoordinator2 = new AclUser
            {
                IdUser = 9,
                Login = "coordinator2",
                Roles = new List<AclRole> { AclRoles.First(r => r.IdRole == 8) },
                IdDepartment = 24,
                Department = Departments.First(r => r.IdDepartment == 24),
                AclDepartments = new List<Department>(),
                Requests = new List<Request>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestsExtComments = new List<RequestExtComment>(),
                Deleted = true
            };
            Departments.First(r => r.IdDepartment == 24).Users.Add(userCoordinator2);
            AclRoles.First(r => r.IdRole == 8).Users.Add(userCoordinator2);
            AclUsers.Add(userCoordinator2);

            // Other users
            var userDispatcher1 = new AclUser
            {
                IdUser = 10,
                Login = "dispatcher1",
                Roles = new List<AclRole> { AclRoles.First(r => r.IdRole == 4) },
                IdDepartment = 24,
                Department = Departments.First(r => r.IdDepartment == 24),
                AclDepartments = new List<Department>(),
                Requests = new List<Request>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestsExtComments = new List<RequestExtComment>()
            };
            Departments.First(r => r.IdDepartment == 24).Users.Add(userDispatcher1);
            AclRoles.First(r => r.IdRole == 4).Users.Add(userDispatcher1);
            AclUsers.Add(userDispatcher1);
            var userRegistrar1 = new AclUser
            {
                IdUser = 11,
                Login = "registrar1",
                Roles = new List<AclRole> { AclRoles.First(r => r.IdRole == 5) },
                IdDepartment = 24,
                Department = Departments.First(r => r.IdDepartment == 24),
                AclDepartments = new List<Department>(),
                Requests = new List<Request>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestsExtComments = new List<RequestExtComment>()
            };
            Departments.First(r => r.IdDepartment == 24).Users.Add(userRegistrar1);
            AclRoles.First(r => r.IdRole == 5).Users.Add(userRegistrar1);
            AclUsers.Add(userRegistrar1);
            var userExecutor1 = new AclUser
            {
                IdUser = 12,
                Login = "executor1",
                Roles = new List<AclRole> { AclRoles.First(r => r.IdRole == 6) },
                IdDepartment = 24,
                Department = Departments.First(r => r.IdDepartment == 24),
                AclDepartments = new List<Department>(),
                Requests = new List<Request>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestsExtComments = new List<RequestExtComment>()
            };
            Departments.First(r => r.IdDepartment == 24).Users.Add(userExecutor1);
            AclRoles.First(r => r.IdRole == 6).Users.Add(userExecutor1);
            AclUsers.Add(userExecutor1);
            var userResourceManager1 = new AclUser
            {
                IdUser = 13,
                Login = "resourceManager1",
                Roles = new List<AclRole> { AclRoles.First(r => r.IdRole == 7) },
                IdDepartment = 24,
                Department = Departments.First(r => r.IdDepartment == 24),
                AclDepartments = new List<Department>(),
                Requests = new List<Request>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestsExtComments = new List<RequestExtComment>()
            };
            Departments.First(r => r.IdDepartment == 24).Users.Add(userResourceManager1);
            AclRoles.First(r => r.IdRole == 7).Users.Add(userResourceManager1);
            AclUsers.Add(userResourceManager1);
        }

        private void LoadRequestStateTypes()
        {
            var requestStateType1 = new RequestStateType { IdRequestStateType = 1, RequestStates = new List<RequestState>() };
            RequestStateTypes.Add(requestStateType1);
            var requestStateType2 = new RequestStateType { IdRequestStateType = 2, RequestStates = new List<RequestState>() };
            RequestStateTypes.Add(requestStateType2);
            var requestStateType3 = new RequestStateType { IdRequestStateType = 3, RequestStates = new List<RequestState>() };
            RequestStateTypes.Add(requestStateType3);
            var requestStateType4 = new RequestStateType { IdRequestStateType = 4, RequestStates = new List<RequestState>() };
            RequestStateTypes.Add(requestStateType4);
            var requestStateType5 = new RequestStateType { IdRequestStateType = 5, RequestStates = new List<RequestState>() };
            RequestStateTypes.Add(requestStateType5);
        }

        private void LoadRequestTypes()
        {
            var requestType1 = new RequestType { IdRequestType = 1, Requests = new List<Request>() };
            RequestTypes.Add(requestType1);
            var requestType2 = new RequestType { IdRequestType = 2, Requests = new List<Request>() };
            RequestTypes.Add(requestType2);
            var requestType3 = new RequestType { IdRequestType = 3, Requests = new List<Request>() };
            RequestTypes.Add(requestType3);
            var requestType4 = new RequestType { IdRequestType = 4, Requests = new List<Request>() };
            RequestTypes.Add(requestType4);
        }

        private void LoadRequests()
        {
            var request1 = new Request
            {
                IdRequest = 1,
                User = AclUsers.First(r => r.IdUser == 3),
                IdRequestType = 1,
                RequestType = RequestTypes.First(r => r.IdRequestType == 1),
                RequestStates = new List<RequestState>(),
                RequestUserAssoc = new List<RequestUserAssoc>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>()
            };
            AclUsers.First(r => r.IdUser == 3).Requests.Add(request1);
            Requests.Add(request1);
            var request2 = new Request
            {
                IdRequest = 2,
                User = AclUsers.First(r => r.IdUser == 3),
                IdRequestType = 2,
                RequestType = RequestTypes.First(r => r.IdRequestType == 2),
                RequestStates = new List<RequestState>(),
                RequestUserAssoc = new List<RequestUserAssoc>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>()
            };
            AclUsers.First(r => r.IdUser == 3).Requests.Add(request2);
            Requests.Add(request2);
            var request3 = new Request
            {
                IdRequest = 3,
                User = AclUsers.First(r => r.IdUser == 4),
                IdRequestType = 3,
                RequestType = RequestTypes.First(r => r.IdRequestType == 4),
                RequestStates = new List<RequestState>(),
                RequestUserAssoc = new List<RequestUserAssoc>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>()
            };
            AclUsers.First(r => r.IdUser == 4).Requests.Add(request3);
            Requests.Add(request3);
            var request4 = new Request
            {
                IdRequest = 4,
                User = AclUsers.First(r => r.IdUser == 4),
                IdRequestType = 4,
                RequestType = RequestTypes.First(r => r.IdRequestType == 3),
                RequestStates = new List<RequestState>(),
                RequestUserAssoc = new List<RequestUserAssoc>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>(),
                Deleted = true
            };
            AclUsers.First(r => r.IdUser == 4).Requests.Add(request4);
            Requests.Add(request4);
            var request5 = new Request
            {
                IdRequest = 5,
                User = AclUsers.First(r => r.IdUser == 5),
                IdRequestType = 1,
                RequestType = RequestTypes.First(r => r.IdRequestType == 1),
                RequestStates = new List<RequestState>(),
                RequestUserAssoc = new List<RequestUserAssoc>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>()
            };
            AclUsers.First(r => r.IdUser == 5).Requests.Add(request5);
            Requests.Add(request5);
            var request6 = new Request
            {
                IdRequest = 6,
                User = AclUsers.First(r => r.IdUser == 3),
                IdRequestType = 2,
                RequestType = RequestTypes.First(r => r.IdRequestType == 2),
                RequestStates = new List<RequestState>(),
                RequestUserAssoc = new List<RequestUserAssoc>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>()
            };
            AclUsers.First(r => r.IdUser == 3).Requests.Add(request6);
            Requests.Add(request6);
            var request7 = new Request
            {
                IdRequest = 7,
                User = AclUsers.First(r => r.IdUser == 3),
                IdRequestType = 3,
                RequestType = RequestTypes.First(r => r.IdRequestType == 4),
                RequestStates = new List<RequestState>(),
                RequestUserAssoc = new List<RequestUserAssoc>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>()
            };
            AclUsers.First(r => r.IdUser == 3).Requests.Add(request7);
            Requests.Add(request7);
            var request8 = new Request
            {
                IdRequest = 8,
                User = AclUsers.First(r => r.IdUser == 4),
                IdRequestType = 4,
                RequestType = RequestTypes.First(r => r.IdRequestType == 3),
                RequestStates = new List<RequestState>(),
                RequestUserAssoc = new List<RequestUserAssoc>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>()
            };
            AclUsers.First(r => r.IdUser == 4).Requests.Add(request8);
            Requests.Add(request8);
            var request9 = new Request
            {
                IdRequest = 9,
                User = AclUsers.First(r => r.IdUser == 4),
                IdRequestType = 1,
                RequestType = RequestTypes.First(r => r.IdRequestType == 1),
                RequestStates = new List<RequestState>(),
                RequestUserAssoc = new List<RequestUserAssoc>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>()
            };
            AclUsers.First(r => r.IdUser == 4).Requests.Add(request9);
            Requests.Add(request9);
            var request10 = new Request
            {
                IdRequest = 10,
                User = AclUsers.First(r => r.IdUser == 5),
                IdRequestType = 2,
                RequestType = RequestTypes.First(r => r.IdRequestType == 2),
                RequestStates = new List<RequestState>(),
                RequestUserAssoc = new List<RequestUserAssoc>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>()
            };
            AclUsers.First(r => r.IdUser == 5).Requests.Add(request10);
            Requests.Add(request10);
            var request11 = new Request
            {
                IdRequest = 11,
                User = AclUsers.First(r => r.IdUser == 5),
                IdRequestType = 3,
                RequestType = RequestTypes.First(r => r.IdRequestType == 4),
                RequestStates = new List<RequestState>(),
                RequestUserAssoc = new List<RequestUserAssoc>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>()
            };
            AclUsers.First(r => r.IdUser == 5).Requests.Add(request11);
            Requests.Add(request11);
            var request12 = new Request
            {
                IdRequest = 12,
                User = AclUsers.First(r => r.IdUser == 5),
                IdRequestType = 4,
                RequestType = RequestTypes.First(r => r.IdRequestType == 3),
                RequestStates = new List<RequestState>(),
                RequestUserAssoc = new List<RequestUserAssoc>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>()
            };
            AclUsers.First(r => r.IdUser == 5).Requests.Add(request12);
            Requests.Add(request12);
            var request13 = new Request
            {
                IdRequest = 13,
                User = AclUsers.First(r => r.IdUser == 4),
                IdRequestType = 1,
                RequestType = RequestTypes.First(r => r.IdRequestType == 1),
                RequestStates = new List<RequestState>(),
                RequestUserAssoc = new List<RequestUserAssoc>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>()
            };
            AclUsers.First(r => r.IdUser == 4).Requests.Add(request13);
            Requests.Add(request13);
            var request14 = new Request
            {
                IdRequest = 14,
                User = AclUsers.First(r => r.IdUser == 4),
                IdRequestType = 2,
                RequestType = RequestTypes.First(r => r.IdRequestType == 2),
                RequestStates = new List<RequestState>(),
                RequestUserAssoc = new List<RequestUserAssoc>(),
                RequestAgreements = new List<RequestAgreement>(),
                RequestUserLastSeens = new List<RequestUserLastSeen>()
            };
            AclUsers.First(r => r.IdUser == 4).Requests.Add(request14);
            Requests.Add(request14);
        }

        private void LoadRequestStates()
        {
            // Request 1
            var request1State1 = new RequestState
            {
                IdRequestState = 1,
                IdRequestStateType = 1,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 1),
                IdRequest = 1,
                Request = Requests.First(r => r.IdRequest == 1),
                Date = DateTime.Now
            };
            RequestStates.Add(request1State1);
            Requests.First(r => r.IdRequest == 1).RequestStates.Add(request1State1);
            RequestStateTypes.First(r => r.IdRequestStateType == 1).RequestStates.Add(request1State1);
            var request1State2 = new RequestState
            {
                IdRequestState = 2,
                IdRequestStateType = 2,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 2),
                IdRequest = 1,
                Request = Requests.First(r => r.IdRequest == 1),
                Date = DateTime.Now,
                Deleted = true
            };
            RequestStates.Add(request1State2);
            Requests.First(r => r.IdRequest == 1).RequestStates.Add(request1State2);
            RequestStateTypes.First(r => r.IdRequestStateType == 2).RequestStates.Add(request1State2);

            // Request 2
            var request2State1 = new RequestState
            {
                IdRequestState = 3,
                IdRequestStateType = 2,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 2),
                IdRequest = 2,
                Request = Requests.First(r => r.IdRequest == 2),
                Date = DateTime.Now
            };
            RequestStates.Add(request2State1);
            Requests.First(r => r.IdRequest == 2).RequestStates.Add(request2State1);
            RequestStateTypes.First(r => r.IdRequestStateType == 2).RequestStates.Add(request2State1);
            var request2State2 = new RequestState
            {
                IdRequestState = 4,
                IdRequestStateType = 3,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 3),
                IdRequest = 2,
                Request = Requests.First(r => r.IdRequest == 2),
                Date = DateTime.Now
            };
            RequestStates.Add(request2State2);
            Requests.First(r => r.IdRequest == 1).RequestStates.Add(request2State2);
            RequestStateTypes.First(r => r.IdRequestStateType == 3).RequestStates.Add(request2State2);

            // Request 3
            var request3State1 = new RequestState
            {
                IdRequestState = 5,
                IdRequestStateType = 1,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 1),
                IdRequest = 3,
                Request = Requests.First(r => r.IdRequest == 3),
                Date = DateTime.Now
            };
            RequestStates.Add(request3State1);
            Requests.First(r => r.IdRequest == 3).RequestStates.Add(request3State1);
            RequestStateTypes.First(r => r.IdRequestStateType == 1).RequestStates.Add(request3State1);
            var request3State2 = new RequestState
            {
                IdRequestState = 6,
                IdRequestStateType = 2,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 2),
                IdRequest = 3,
                Request = Requests.First(r => r.IdRequest == 3),
                Date = DateTime.Now
            };
            RequestStates.Add(request3State2);
            Requests.First(r => r.IdRequest == 3).RequestStates.Add(request3State2);
            RequestStateTypes.First(r => r.IdRequestStateType == 2).RequestStates.Add(request3State2);
            var request3State3 = new RequestState
            {
                IdRequestState = 7,
                IdRequestStateType = 3,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 3),
                IdRequest = 3,
                Request = Requests.First(r => r.IdRequest == 3),
                Date = DateTime.Now
            };
            RequestStates.Add(request3State3);
            Requests.First(r => r.IdRequest == 3).RequestStates.Add(request3State3);
            RequestStateTypes.First(r => r.IdRequestStateType == 3).RequestStates.Add(request3State3);
            var request3State4 = new RequestState
            {
                IdRequestState = 34,
                IdRequestStateType = 4,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 4),
                IdRequest = 3,
                Request = Requests.First(r => r.IdRequest == 3),
                Date = DateTime.Now
            };
            RequestStates.Add(request3State4);
            Requests.First(r => r.IdRequest == 3).RequestStates.Add(request3State4);
            RequestStateTypes.First(r => r.IdRequestStateType == 4).RequestStates.Add(request3State4);

            // Request 4
            var request4State1 = new RequestState
            {
                IdRequestState = 8,
                IdRequestStateType = 2,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 2),
                IdRequest = 4,
                Request = Requests.First(r => r.IdRequest == 4),
                Date = DateTime.Now
            };
            RequestStates.Add(request4State1);
            Requests.First(r => r.IdRequest == 4).RequestStates.Add(request4State1);
            RequestStateTypes.First(r => r.IdRequestStateType == 2).RequestStates.Add(request4State1);

            // Request 5
            var request5State1 = new RequestState
            {
                IdRequestState = 9,
                IdRequestStateType = 1,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 1),
                IdRequest = 5,
                Request = Requests.First(r => r.IdRequest == 5),
                Date = DateTime.Now
            };
            RequestStates.Add(request5State1);
            Requests.First(r => r.IdRequest == 5).RequestStates.Add(request5State1);
            RequestStateTypes.First(r => r.IdRequestStateType == 1).RequestStates.Add(request5State1);
            var request5State2 = new RequestState
            {
                IdRequestState = 10,
                IdRequestStateType = 5,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 5),
                IdRequest = 5,
                Request = Requests.First(r => r.IdRequest == 5),
                Date = DateTime.Now
            };
            RequestStates.Add(request5State2);
            Requests.First(r => r.IdRequest == 5).RequestStates.Add(request5State2);
            RequestStateTypes.First(r => r.IdRequestStateType == 5).RequestStates.Add(request5State2);

            // Request 6
            var request6State1 = new RequestState
            {
                IdRequestState = 11,
                IdRequestStateType = 1,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 1),
                IdRequest = 6,
                Request = Requests.First(r => r.IdRequest == 6),
                Date = DateTime.Now
            };
            RequestStates.Add(request6State1);
            Requests.First(r => r.IdRequest == 6).RequestStates.Add(request6State1);
            RequestStateTypes.First(r => r.IdRequestStateType == 1).RequestStates.Add(request6State1);
            var request6State2 = new RequestState
            {
                IdRequestState = 12,
                IdRequestStateType = 5,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 5),
                IdRequest = 6,
                Request = Requests.First(r => r.IdRequest == 6),
                Date = DateTime.Now
            };
            RequestStates.Add(request6State2);
            Requests.First(r => r.IdRequest == 6).RequestStates.Add(request6State2);
            RequestStateTypes.First(r => r.IdRequestStateType == 5).RequestStates.Add(request6State2);

            // Request 7
            var request7State1 = new RequestState
            {
                IdRequestState = 13,
                IdRequestStateType = 2,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 2),
                IdRequest = 7,
                Request = Requests.First(r => r.IdRequest == 7),
                Date = DateTime.Now
            };
            RequestStates.Add(request7State1);
            Requests.First(r => r.IdRequest == 7).RequestStates.Add(request7State1);
            RequestStateTypes.First(r => r.IdRequestStateType == 2).RequestStates.Add(request7State1);
            var request7State2 = new RequestState
            {
                IdRequestState = 14,
                IdRequestStateType = 5,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 5),
                IdRequest = 7,
                Request = Requests.First(r => r.IdRequest == 7),
                Date = DateTime.Now
            };
            RequestStates.Add(request7State2);
            Requests.First(r => r.IdRequest == 7).RequestStates.Add(request7State2);
            RequestStateTypes.First(r => r.IdRequestStateType == 5).RequestStates.Add(request7State2);

            // Request 8
            var request8State1 = new RequestState
            {
                IdRequestState = 15,
                IdRequestStateType = 2,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 2),
                IdRequest = 8,
                Request = Requests.First(r => r.IdRequest == 8),
                Date = DateTime.Now
            };
            RequestStates.Add(request8State1);
            Requests.First(r => r.IdRequest == 8).RequestStates.Add(request8State1);
            RequestStateTypes.First(r => r.IdRequestStateType == 2).RequestStates.Add(request8State1);
            var request8State2 = new RequestState
            {
                IdRequestState = 16,
                IdRequestStateType = 1,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 1),
                IdRequest = 8,
                Request = Requests.First(r => r.IdRequest == 8),
                Date = DateTime.Now
            };
            RequestStates.Add(request8State2);
            Requests.First(r => r.IdRequest == 8).RequestStates.Add(request8State2);
            RequestStateTypes.First(r => r.IdRequestStateType == 1).RequestStates.Add(request8State2);

            // Request 9
            var request9State1 = new RequestState
            {
                IdRequestState = 17,
                IdRequestStateType = 1,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 1),
                IdRequest = 9,
                Request = Requests.First(r => r.IdRequest == 9),
                Date = DateTime.Now
            };
            RequestStates.Add(request9State1);
            Requests.First(r => r.IdRequest == 9).RequestStates.Add(request9State1);
            RequestStateTypes.First(r => r.IdRequestStateType == 1).RequestStates.Add(request9State1);
            var request9State2 = new RequestState
            {
                IdRequestState = 18,
                IdRequestStateType = 2,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 2),
                IdRequest = 9,
                Request = Requests.First(r => r.IdRequest == 9),
                Date = DateTime.Now
            };
            RequestStates.Add(request9State2);
            Requests.First(r => r.IdRequest == 9).RequestStates.Add(request9State2);
            RequestStateTypes.First(r => r.IdRequestStateType == 2).RequestStates.Add(request9State2);
            var request9State3 = new RequestState
            {
                IdRequestState = 19,
                IdRequestStateType = 1,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 1),
                IdRequest = 9,
                Request = Requests.First(r => r.IdRequest == 9),
                Date = DateTime.Now
            };
            RequestStates.Add(request9State3);
            Requests.First(r => r.IdRequest == 9).RequestStates.Add(request9State3);
            RequestStateTypes.First(r => r.IdRequestStateType == 1).RequestStates.Add(request9State3);
            var request9State4 = new RequestState
            {
                IdRequestState = 20,
                IdRequestStateType = 2,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 2),
                IdRequest = 9,
                Request = Requests.First(r => r.IdRequest == 9),
                Date = DateTime.Now
            };
            RequestStates.Add(request9State4);
            Requests.First(r => r.IdRequest == 9).RequestStates.Add(request9State4);
            RequestStateTypes.First(r => r.IdRequestStateType == 2).RequestStates.Add(request9State4);

            // Request 10
            var request10State1 = new RequestState
            {
                IdRequestState = 21,
                IdRequestStateType = 1,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 1),
                IdRequest = 10,
                Request = Requests.First(r => r.IdRequest == 10),
                Date = DateTime.Now
            };
            RequestStates.Add(request10State1);
            Requests.First(r => r.IdRequest == 10).RequestStates.Add(request10State1);
            RequestStateTypes.First(r => r.IdRequestStateType == 1).RequestStates.Add(request10State1);
            var request10State2 = new RequestState
            {
                IdRequestState = 22,
                IdRequestStateType = 2,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 2),
                IdRequest = 10,
                Request = Requests.First(r => r.IdRequest == 10),
                Date = DateTime.Now
            };
            RequestStates.Add(request10State2);
            Requests.First(r => r.IdRequest == 10).RequestStates.Add(request10State2);
            RequestStateTypes.First(r => r.IdRequestStateType == 2).RequestStates.Add(request10State2);
            var request10State3 = new RequestState
            {
                IdRequestState = 23,
                IdRequestStateType = 1,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 1),
                IdRequest = 10,
                Request = Requests.First(r => r.IdRequest == 10),
                Date = DateTime.Now
            };
            RequestStates.Add(request10State3);
            Requests.First(r => r.IdRequest == 10).RequestStates.Add(request10State3);
            RequestStateTypes.First(r => r.IdRequestStateType == 1).RequestStates.Add(request10State3);
            var request10State4 = new RequestState
            {
                IdRequestState = 24,
                IdRequestStateType = 2,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 2),
                IdRequest = 10,
                Request = Requests.First(r => r.IdRequest == 10),
                Date = DateTime.Now
            };
            RequestStates.Add(request10State4);
            Requests.First(r => r.IdRequest == 10).RequestStates.Add(request10State4);
            RequestStateTypes.First(r => r.IdRequestStateType == 2).RequestStates.Add(request10State4);
            var request10State5 = new RequestState
            {
                IdRequestState = 25,
                IdRequestStateType = 3,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 3),
                IdRequest = 10,
                Request = Requests.First(r => r.IdRequest == 10),
                Date = DateTime.Now
            };
            RequestStates.Add(request10State5);
            Requests.First(r => r.IdRequest == 10).RequestStates.Add(request10State5);
            RequestStateTypes.First(r => r.IdRequestStateType == 3).RequestStates.Add(request10State5);

            // Request 11
            var request11State1 = new RequestState
            {
                IdRequestState = 26,
                IdRequestStateType = 1,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 1),
                IdRequest = 11,
                Request = Requests.First(r => r.IdRequest == 11),
                Date = DateTime.Now
            };
            RequestStates.Add(request11State1);
            Requests.First(r => r.IdRequest == 11).RequestStates.Add(request11State1);
            RequestStateTypes.First(r => r.IdRequestStateType == 1).RequestStates.Add(request11State1);
            var request11State2 = new RequestState
            {
                IdRequestState = 27,
                IdRequestStateType = 2,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 2),
                IdRequest = 11,
                Request = Requests.First(r => r.IdRequest == 11),
                Date = DateTime.Now
            };
            RequestStates.Add(request11State2);
            Requests.First(r => r.IdRequest == 11).RequestStates.Add(request11State2);
            RequestStateTypes.First(r => r.IdRequestStateType == 2).RequestStates.Add(request11State2);
            var request11State3 = new RequestState
            {
                IdRequestState = 28,
                IdRequestStateType = 1,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 1),
                IdRequest = 11,
                Request = Requests.First(r => r.IdRequest == 11),
                Date = DateTime.Now
            };
            RequestStates.Add(request11State3);
            Requests.First(r => r.IdRequest == 11).RequestStates.Add(request11State3);
            RequestStateTypes.First(r => r.IdRequestStateType == 1).RequestStates.Add(request11State3);
            var request11State4 = new RequestState
            {
                IdRequestState = 29,
                IdRequestStateType = 5,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 5),
                IdRequest = 11,
                Request = Requests.First(r => r.IdRequest == 11),
                Date = DateTime.Now
            };
            RequestStates.Add(request11State4);
            Requests.First(r => r.IdRequest == 11).RequestStates.Add(request11State4);
            RequestStateTypes.First(r => r.IdRequestStateType == 5).RequestStates.Add(request11State4);

            // Request 12
            var request12State1 = new RequestState
            {
                IdRequestState = 30,
                IdRequestStateType = 2,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 2),
                IdRequest = 12,
                Request = Requests.First(r => r.IdRequest == 12),
                Date = DateTime.Now
            };
            RequestStates.Add(request12State1);
            Requests.First(r => r.IdRequest == 12).RequestStates.Add(request12State1);
            RequestStateTypes.First(r => r.IdRequestStateType == 2).RequestStates.Add(request12State1);

            // Request 13
            var request13State1 = new RequestState
            {
                IdRequestState = 31,
                IdRequestStateType = 2,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 2),
                IdRequest = 13,
                Request = Requests.First(r => r.IdRequest == 13),
                Date = DateTime.Now
            };
            RequestStates.Add(request13State1);
            Requests.First(r => r.IdRequest == 13).RequestStates.Add(request13State1);
            RequestStateTypes.First(r => r.IdRequestStateType == 2).RequestStates.Add(request13State1);
            var request13State2 = new RequestState
            {
                IdRequestState = 32,
                IdRequestStateType = 3,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 3),
                IdRequest = 13,
                Request = Requests.First(r => r.IdRequest == 13),
                Date = DateTime.Now
            };
            RequestStates.Add(request13State2);
            Requests.First(r => r.IdRequest == 13).RequestStates.Add(request13State2);
            RequestStateTypes.First(r => r.IdRequestStateType == 3).RequestStates.Add(request13State2);
            var request13State3 = new RequestState
            {
                IdRequestState = 33,
                IdRequestStateType = 4,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 4),
                IdRequest = 13,
                Request = Requests.First(r => r.IdRequest == 13),
                Date = DateTime.Now
            };
            RequestStates.Add(request13State3);
            Requests.First(r => r.IdRequest == 13).RequestStates.Add(request13State3);
            RequestStateTypes.First(r => r.IdRequestStateType == 4).RequestStates.Add(request13State3);
            var request13State4 = new RequestState
            {
                IdRequestState = 35,
                IdRequestStateType = 5,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 5),
                IdRequest = 13,
                Request = Requests.First(r => r.IdRequest == 13),
                Date = DateTime.Now
            };
            RequestStates.Add(request13State4);
            Requests.First(r => r.IdRequest == 13).RequestStates.Add(request13State4);
            RequestStateTypes.First(r => r.IdRequestStateType == 5).RequestStates.Add(request13State4);

            // Request 14
            var request14State1 = new RequestState
            {
                IdRequestState = 36,
                IdRequestStateType = 1,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 1),
                IdRequest = 14,
                Request = Requests.First(r => r.IdRequest == 14),
                Date = DateTime.Now
            };
            RequestStates.Add(request14State1);
            Requests.First(r => r.IdRequest == 14).RequestStates.Add(request14State1);
            RequestStateTypes.First(r => r.IdRequestStateType == 1).RequestStates.Add(request14State1);
            var request14State2 = new RequestState
            {
                IdRequestState = 37,
                IdRequestStateType = 2,
                RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == 2),
                IdRequest = 14,
                Request = Requests.First(r => r.IdRequest == 14),
                Date = DateTime.Now
            };
            RequestStates.Add(request14State2);
            Requests.First(r => r.IdRequest == 14).RequestStates.Add(request14State2);
            RequestStateTypes.First(r => r.IdRequestStateType == 2).RequestStates.Add(request14State2);
        }

        private void LoadRequestUsers()
        {
            var requestUser1 = new RequestUser
            {
                IdRequestUser = 1,
                Login = "user1",
                RequestUserAssoc = new List<RequestUserAssoc>()
            };
            Users.Add(requestUser1);
            var requestUser2 = new RequestUser
            {
                IdRequestUser = 2,
                Login = "user2",
                RequestUserAssoc = new List<RequestUserAssoc>()
            };
            Users.Add(requestUser2);
            var requestUser3 = new RequestUser
            {
                IdRequestUser = 3,
                Login = "user3",
                RequestUserAssoc = new List<RequestUserAssoc>()
            };
            Users.Add(requestUser3);
            var requestUser4 = new RequestUser
            {
                IdRequestUser = 4,
                Login = "user4",
                RequestUserAssoc = new List<RequestUserAssoc>()
            };
            Users.Add(requestUser4);
            var requestUser5 = new RequestUser
            {
                IdRequestUser = 5,
                Login = "user5",
                RequestUserAssoc = new List<RequestUserAssoc>()
            };
            Users.Add(requestUser5);
            var requestUser6 = new RequestUser
            {
                IdRequestUser = 6,
                Login = "user6",
                RequestUserAssoc = new List<RequestUserAssoc>(),
                Deleted = true
            };
            Users.Add(requestUser6);
            var requestUser7 = new RequestUser
            {
                IdRequestUser = 7,
                Login = "user6",    // same login
                RequestUserAssoc = new List<RequestUserAssoc>()
            };
            Users.Add(requestUser7);
        }

        private void LoadRequestUserAssocs()
        {
            var assoc1 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 1,
                IdRequest = 1,
                Request = Requests.First(r => r.IdRequest == 1),
                IdRequestUser = 1,
                RequestUser = Users.First(r => r.IdRequestUser == 1),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc1);
            Users.First(r => r.IdRequestUser == 1).RequestUserAssoc.Add(assoc1);
            Requests.First(r => r.IdRequest == 1).RequestUserAssoc.Add(assoc1);
            var assoc2 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 2,
                IdRequest = 1,
                Request = Requests.First(r => r.IdRequest == 1),
                IdRequestUser = 2,
                RequestUser = Users.First(r => r.IdRequestUser == 2),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc2);
            Users.First(r => r.IdRequestUser == 2).RequestUserAssoc.Add(assoc2);
            Requests.First(r => r.IdRequest == 1).RequestUserAssoc.Add(assoc2);
            var assoc3 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 3,
                IdRequest = 1,
                Request = Requests.First(r => r.IdRequest == 1),
                IdRequestUser = 3,
                RequestUser = Users.First(r => r.IdRequestUser == 3),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>(),
                Deleted = true
            };
            RequestUserAssocs.Add(assoc3);
            Users.First(r => r.IdRequestUser == 3).RequestUserAssoc.Add(assoc3);
            Requests.First(r => r.IdRequest == 1).RequestUserAssoc.Add(assoc3);
            var assoc4 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 4,
                IdRequest = 2,
                Request = Requests.First(r => r.IdRequest == 2),
                IdRequestUser = 2,
                RequestUser = Users.First(r => r.IdRequestUser == 2),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc4);
            Users.First(r => r.IdRequestUser == 2).RequestUserAssoc.Add(assoc4);
            Requests.First(r => r.IdRequest == 2).RequestUserAssoc.Add(assoc4);
            var assoc5 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 5,
                IdRequest = 2,
                Request = Requests.First(r => r.IdRequest == 2),
                IdRequestUser = 3,
                RequestUser = Users.First(r => r.IdRequestUser == 3),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc5);
            Users.First(r => r.IdRequestUser == 3).RequestUserAssoc.Add(assoc5);
            Requests.First(r => r.IdRequest == 2).RequestUserAssoc.Add(assoc5);
            var assoc6 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 6,
                IdRequest = 3,
                Request = Requests.First(r => r.IdRequest == 3),
                IdRequestUser = 4,
                RequestUser = Users.First(r => r.IdRequestUser == 4),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc6);
            Users.First(r => r.IdRequestUser == 4).RequestUserAssoc.Add(assoc6);
            Requests.First(r => r.IdRequest == 3).RequestUserAssoc.Add(assoc6);
            var assoc7 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 7,
                IdRequest = 4,
                Request = Requests.First(r => r.IdRequest == 4),
                IdRequestUser = 5,
                RequestUser = Users.First(r => r.IdRequestUser == 5),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc7);
            Users.First(r => r.IdRequestUser == 5).RequestUserAssoc.Add(assoc7);
            Requests.First(r => r.IdRequest == 4).RequestUserAssoc.Add(assoc7);
            var assoc8 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 8,
                IdRequest = 5,
                Request = Requests.First(r => r.IdRequest == 5),
                IdRequestUser = 5,
                RequestUser = Users.First(r => r.IdRequestUser == 5),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc8);
            Users.First(r => r.IdRequestUser == 5).RequestUserAssoc.Add(assoc8);
            Requests.First(r => r.IdRequest == 5).RequestUserAssoc.Add(assoc8);
            var assoc9 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 9,
                IdRequest = 5,
                Request = Requests.First(r => r.IdRequest == 5),
                IdRequestUser = 7,
                RequestUser = Users.First(r => r.IdRequestUser == 7),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc9);
            Users.First(r => r.IdRequestUser == 7).RequestUserAssoc.Add(assoc9);
            Requests.First(r => r.IdRequest == 5).RequestUserAssoc.Add(assoc9);
            var assoc10 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 10,
                IdRequest = 6,
                Request = Requests.First(r => r.IdRequest == 6),
                IdRequestUser = 5,
                RequestUser = Users.First(r => r.IdRequestUser == 5),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc10);
            Users.First(r => r.IdRequestUser == 5).RequestUserAssoc.Add(assoc10);
            Requests.First(r => r.IdRequest == 6).RequestUserAssoc.Add(assoc10);
            var assoc11 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 11,
                IdRequest = 6,
                Request = Requests.First(r => r.IdRequest == 6),
                IdRequestUser = 6,
                RequestUser = Users.First(r => r.IdRequestUser == 6),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc11);
            Users.First(r => r.IdRequestUser == 6).RequestUserAssoc.Add(assoc11);
            Requests.First(r => r.IdRequest == 6).RequestUserAssoc.Add(assoc11);
            var assoc12 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 12,
                IdRequest = 7,
                Request = Requests.First(r => r.IdRequest == 7),
                IdRequestUser = 1,
                RequestUser = Users.First(r => r.IdRequestUser == 1),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc12);
            Users.First(r => r.IdRequestUser == 1).RequestUserAssoc.Add(assoc12);
            Requests.First(r => r.IdRequest == 7).RequestUserAssoc.Add(assoc12);
            var assoc13 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 13,
                IdRequest = 7,
                Request = Requests.First(r => r.IdRequest == 7),
                IdRequestUser = 2,
                RequestUser = Users.First(r => r.IdRequestUser == 2),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc13);
            Users.First(r => r.IdRequestUser == 2).RequestUserAssoc.Add(assoc13);
            Requests.First(r => r.IdRequest == 7).RequestUserAssoc.Add(assoc13);
            var assoc14 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 14,
                IdRequest = 7,
                Request = Requests.First(r => r.IdRequest == 7),
                IdRequestUser = 4,
                RequestUser = Users.First(r => r.IdRequestUser == 4),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc14);
            Users.First(r => r.IdRequestUser == 4).RequestUserAssoc.Add(assoc14);
            Requests.First(r => r.IdRequest == 7).RequestUserAssoc.Add(assoc14);
            var assoc15 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 15,
                IdRequest = 8,
                Request = Requests.First(r => r.IdRequest == 8),
                IdRequestUser = 6,
                RequestUser = Users.First(r => r.IdRequestUser == 6),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc15);
            Users.First(r => r.IdRequestUser == 6).RequestUserAssoc.Add(assoc15);
            Requests.First(r => r.IdRequest == 8).RequestUserAssoc.Add(assoc15);
            var assoc16 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 16,
                IdRequest = 9,
                Request = Requests.First(r => r.IdRequest == 9),
                IdRequestUser = 1,
                RequestUser = Users.First(r => r.IdRequestUser == 1),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc16);
            Users.First(r => r.IdRequestUser == 1).RequestUserAssoc.Add(assoc16);
            Requests.First(r => r.IdRequest == 9).RequestUserAssoc.Add(assoc16);
            var assoc17 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 17,
                IdRequest = 9,
                Request = Requests.First(r => r.IdRequest == 9),
                IdRequestUser = 2,
                RequestUser = Users.First(r => r.IdRequestUser == 2),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc17);
            Users.First(r => r.IdRequestUser == 2).RequestUserAssoc.Add(assoc17);
            Requests.First(r => r.IdRequest == 9).RequestUserAssoc.Add(assoc17);
            var assoc18 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 18,
                IdRequest = 9,
                Request = Requests.First(r => r.IdRequest == 9),
                IdRequestUser = 3,
                RequestUser = Users.First(r => r.IdRequestUser == 3),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc18);
            Users.First(r => r.IdRequestUser == 3).RequestUserAssoc.Add(assoc18);
            Requests.First(r => r.IdRequest == 9).RequestUserAssoc.Add(assoc18);
            var assoc19 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 19,
                IdRequest = 9,
                Request = Requests.First(r => r.IdRequest == 9),
                IdRequestUser = 4,
                RequestUser = Users.First(r => r.IdRequestUser == 4),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc19);
            Users.First(r => r.IdRequestUser == 4).RequestUserAssoc.Add(assoc19);
            Requests.First(r => r.IdRequest == 9).RequestUserAssoc.Add(assoc19);
            var assoc20 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 20,
                IdRequest = 9,
                Request = Requests.First(r => r.IdRequest == 9),
                IdRequestUser = 5,
                RequestUser = Users.First(r => r.IdRequestUser == 5),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc20);
            Users.First(r => r.IdRequestUser == 5).RequestUserAssoc.Add(assoc20);
            Requests.First(r => r.IdRequest == 9).RequestUserAssoc.Add(assoc20);
            var assoc21 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 21,
                IdRequest = 10,
                Request = Requests.First(r => r.IdRequest == 10),
                IdRequestUser = 4,
                RequestUser = Users.First(r => r.IdRequestUser == 4),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc21);
            Users.First(r => r.IdRequestUser == 4).RequestUserAssoc.Add(assoc21);
            Requests.First(r => r.IdRequest == 10).RequestUserAssoc.Add(assoc21);
            var assoc22 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 22,
                IdRequest = 10,
                Request = Requests.First(r => r.IdRequest == 10),
                IdRequestUser = 7,
                RequestUser = Users.First(r => r.IdRequestUser == 7),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc22);
            Users.First(r => r.IdRequestUser == 7).RequestUserAssoc.Add(assoc22);
            Requests.First(r => r.IdRequest == 10).RequestUserAssoc.Add(assoc22);
            var assoc23 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 23,
                IdRequest = 11,
                Request = Requests.First(r => r.IdRequest == 11),
                IdRequestUser = 3,
                RequestUser = Users.First(r => r.IdRequestUser == 3),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc23);
            Users.First(r => r.IdRequestUser == 3).RequestUserAssoc.Add(assoc23);
            Requests.First(r => r.IdRequest == 11).RequestUserAssoc.Add(assoc23);
            var assoc24 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 24,
                IdRequest = 11,
                Request = Requests.First(r => r.IdRequest == 11),
                IdRequestUser = 4,
                RequestUser = Users.First(r => r.IdRequestUser == 4),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc24);
            Users.First(r => r.IdRequestUser == 4).RequestUserAssoc.Add(assoc24);
            Requests.First(r => r.IdRequest == 11).RequestUserAssoc.Add(assoc24);
            var assoc25 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 25,
                IdRequest = 11,
                Request = Requests.First(r => r.IdRequest == 11),
                IdRequestUser = 7,
                RequestUser = Users.First(r => r.IdRequestUser == 7),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc25);
            Users.First(r => r.IdRequestUser == 7).RequestUserAssoc.Add(assoc25);
            Requests.First(r => r.IdRequest == 11).RequestUserAssoc.Add(assoc25);
            var assoc26 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 26,
                IdRequest = 12,
                Request = Requests.First(r => r.IdRequest == 12),
                IdRequestUser = 4,
                RequestUser = Users.First(r => r.IdRequestUser == 4),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc26);
            Users.First(r => r.IdRequestUser == 4).RequestUserAssoc.Add(assoc26);
            Requests.First(r => r.IdRequest == 12).RequestUserAssoc.Add(assoc26);
            var assoc27 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 27,
                IdRequest = 12,
                Request = Requests.First(r => r.IdRequest == 12),
                IdRequestUser = 5,
                RequestUser = Users.First(r => r.IdRequestUser == 5),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc27);
            Users.First(r => r.IdRequestUser == 5).RequestUserAssoc.Add(assoc27);
            Requests.First(r => r.IdRequest == 12).RequestUserAssoc.Add(assoc27);
            var assoc28 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 28,
                IdRequest = 13,
                Request = Requests.First(r => r.IdRequest == 13),
                IdRequestUser = 2,
                RequestUser = Users.First(r => r.IdRequestUser == 2),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc28);
            Users.First(r => r.IdRequestUser == 2).RequestUserAssoc.Add(assoc28);
            Requests.First(r => r.IdRequest == 13).RequestUserAssoc.Add(assoc28);
            var assoc29 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 29,
                IdRequest = 13,
                Request = Requests.First(r => r.IdRequest == 13),
                IdRequestUser = 4,
                RequestUser = Users.First(r => r.IdRequestUser == 4),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc28);
            Users.First(r => r.IdRequestUser == 4).RequestUserAssoc.Add(assoc29);
            Requests.First(r => r.IdRequest == 13).RequestUserAssoc.Add(assoc29);
            var assoc30 = new RequestUserAssoc
            {
                IdRequestUserAssoc = 30,
                IdRequest = 14,
                Request = Requests.First(r => r.IdRequest == 14),
                IdRequestUser = 1,
                RequestUser = Users.First(r => r.IdRequestUser == 1),
                RequestUserRightAssocs = new List<RequestUserRightAssoc>()
            };
            RequestUserAssocs.Add(assoc30);
            Users.First(r => r.IdRequestUser == 1).RequestUserAssoc.Add(assoc30);
            Requests.First(r => r.IdRequest == 14).RequestUserAssoc.Add(assoc30);
        }

        private void InitializeCollections()
        {
            ResourceRights = new List<ResourceRight>();
            Resources = new List<Resource>();
            ResourceGroups = new List<ResourceGroup>();
            Departments = new List<Department>();
            AclUsers = new List<AclUser>();
            AclRoles = new List<AclRole>();
            Requests = new List<Request>();
            RequestTypes = new List<RequestType>();
            RequestStateTypes = new List<RequestStateType>();
            Users = new List<RequestUser>();
            RequestUserAssocs = new List<RequestUserAssoc>();
            RequestUserRightAssocs = new List<RequestUserRightAssoc>();
            DelegationRequestUsersExtInfo = new List<DelegationRequestUsersExtInfo>();
            RequestExtComments = new List<RequestExtComment>();
            RequestAgreementTypes = new List<RequestAgreementType>();
            RequestAgreementStates = new List<RequestAgreementState>();
            RequestAgreements = new List<RequestAgreement>();
            RequestUserLastSeens = new List<RequestUserLastSeen>();
            RequestStates = new List<RequestState>();
            RequestRightGrantTypes = new List<RequestRightGrantType>();
            ResourceInformationTypes = new List<ResourceInformationType>();
            ResourceDeviceAddresses = new List<ResourceDeviceAddress>();
            ResourceInternetAddresses = new List<ResourceInternetAddress>();
            ResourceOperatorPersons = new List<ResourceOperatorPerson>();
            ResourceOperatorPersonActs = new List<ResourceOperatorPersonAct>();
            ResourceOwnerPersons = new List<ResourceOwnerPerson>();
            ResourceOwnerPersonActs = new List<ResourceOwnerPersonAct>();
            ResourceAuthorityActs = new List<ResourceAuthorityAct>();
            ResourceOperatorActs = new List<ResourceOperatorAct>();
            ResourceUsingActs = new List<ResourceUsingAct>();
            ActFiles = new List<ActFile>();
        }
    }
}
