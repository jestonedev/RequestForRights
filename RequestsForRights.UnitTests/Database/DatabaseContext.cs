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
            LoadRequestRightGrantType();
            LoadRequestUserRightsAssocs();
            LoadRequestAgreementStates();
            LoadRequestAgreementTypes();
            LoadRequestAgreements();
            LoadRequestUserLastSeens();
        }

        private void LoadRequestUserLastSeens()
        {
            var requestUserLastSeens = new[]
            {
                new { IdRequestUserLastSeen = 1, IdRequest = 1, IdUser = 1 },
                new { IdRequestUserLastSeen = 2, IdRequest = 3, IdUser = 1 },
                new { IdRequestUserLastSeen = 3, IdRequest = 2, IdUser = 2 },
                new { IdRequestUserLastSeen = 4, IdRequest = 4, IdUser = 1 }
            };
            foreach (var requestUserLastSeenProperties in requestUserLastSeens)
            {
                var requestUserLastSeen = new RequestUserLastSeen
                {
                    IdRequestUserLastSeen = requestUserLastSeenProperties.IdRequestUserLastSeen,
                    IdUser = requestUserLastSeenProperties.IdUser,
                    User = AclUsers.First(r => r.IdUser == requestUserLastSeenProperties.IdUser),
                    IdRequest = requestUserLastSeenProperties.IdRequest,
                    Request = Requests.First(r => r.IdRequest == requestUserLastSeenProperties.IdRequest)
                };
                RequestUserLastSeens.Add(requestUserLastSeen);
                AclUsers.First(r => r.IdUser == requestUserLastSeenProperties.IdUser)
                    .RequestUserLastSeens.Add(requestUserLastSeen);
                Requests.First(r => r.IdRequest == requestUserLastSeenProperties.IdRequest)
                    .RequestUserLastSeens.Add(requestUserLastSeen);
            }
        }

        private void LoadRequestAgreementTypes()
        {
            var requestAgreementTypes = new[]
            {
                new { IdAgreementType = 1 },
                new { IdAgreementType = 2 }
            };
            foreach (var requestAgreementTypeParams in requestAgreementTypes)
            {
                RequestAgreementTypes.Add(new RequestAgreementType
                {
                    IdAgreementType = requestAgreementTypeParams.IdAgreementType,
                    RequestAgreements = new List<RequestAgreement>()
                });
            }
        }

        private void LoadRequestAgreementStates()
        {
            var requestAgreementStates = new[]
            {
                new { IdAgreementState = 1 },
                new { IdAgreementState = 2 },
                new { IdAgreementState = 3 }
            };
            foreach (var requestAgreementStateParams in requestAgreementStates)
            {
                RequestAgreementStates.Add(new RequestAgreementState
                {
                    IdAgreementState = requestAgreementStateParams.IdAgreementState,
                    RequestAgreements = new List<RequestAgreement>()
                });
            }
        }

        private void LoadRequestAgreements()
        {
            var agreements = new[]
            {
                new { IdRequestAgreement = 1, IdRequest = 1, IdAgreementState = 1, IdAgreementType = 2, IdUser = 9 },
                new { IdRequestAgreement = 2, IdRequest = 11, IdAgreementState = 3, IdAgreementType = 2, IdUser = 9 },
                new { IdRequestAgreement = 3, IdRequest = 11, IdAgreementState = 1, IdAgreementType = 2, IdUser = 10 },
                new { IdRequestAgreement = 4, IdRequest = 9, IdAgreementState = 2, IdAgreementType = 2, IdUser = 10 },
                new { IdRequestAgreement = 5, IdRequest = 8, IdAgreementState = 1, IdAgreementType = 2, IdUser = 9 },
                new { IdRequestAgreement = 6, IdRequest = 10, IdAgreementState = 2, IdAgreementType = 2, IdUser = 9 },
                new { IdRequestAgreement = 7, IdRequest = 10, IdAgreementState = 2, IdAgreementType = 2, IdUser = 10 },
                new { IdRequestAgreement = 8, IdRequest = 15, IdAgreementState = 2, IdAgreementType = 1, IdUser = 6 }
            };
            foreach (var agreementParams in agreements)
            {
                var agreement = new RequestAgreement
                {
                    IdRequestAgreement = agreementParams.IdRequestAgreement,
                    IdRequest = agreementParams.IdRequest,
                    Request = Requests.First(r => r.IdRequest == agreementParams.IdRequest),
                    IdAgreementState = agreementParams.IdAgreementState,
                    AgreementState = RequestAgreementStates.First(r => r.IdAgreementState == agreementParams.IdAgreementState),
                    IdAgreementType = agreementParams.IdAgreementType,
                    AgreementType = RequestAgreementTypes.First(r => r.IdAgreementType == agreementParams.IdAgreementType),
                    IdUser = agreementParams.IdUser,
                    User = AclUsers.First(r => r.IdUser == agreementParams.IdUser)
                };
                RequestAgreements.Add(agreement);
                Requests.First(r => r.IdRequest == agreementParams.IdRequest).RequestAgreements.Add(agreement);
                RequestAgreementStates.First(r => r.IdAgreementState == agreementParams.IdAgreementState).RequestAgreements.Add(agreement);
                RequestAgreementTypes.First(r => r.IdAgreementType == agreementParams.IdAgreementType).RequestAgreements.Add(agreement);
                AclUsers.First(r => r.IdUser == agreementParams.IdUser).RequestAgreements.Add(agreement);
            }
        }

        private void LoadRequestRightGrantType()
        {
            var rightGrantTypes = new[]
            {
                new { IdRequestRightGrantType = 1 },
                new { IdRequestRightGrantType = 2 },
                new { IdRequestRightGrantType = 3 },
            };
            foreach (var rightGrantTypeParams in rightGrantTypes)
            {
                RequestRightGrantTypes.Add(new RequestRightGrantType
                {
                    IdRequestRightGrantType = rightGrantTypeParams.IdRequestRightGrantType,
                    RequestUserRightAssoc = new List<RequestUserRightAssoc>()
                });
            }
        }

        private void LoadRequestUserRightsAssocs()
        {
            var ruars = new[]
            {
                new { IdAssoc = 1, IdRequestUserAssoc = 1, IdResourceRight = 3, IdRequestRightGrantType = 1 },
                new { IdAssoc = 2, IdRequestUserAssoc = 2, IdResourceRight = 3, IdRequestRightGrantType = 1 },
                new { IdAssoc = 3, IdRequestUserAssoc = 2, IdResourceRight = 4, IdRequestRightGrantType = 2 },
                new { IdAssoc = 4, IdRequestUserAssoc = 3, IdResourceRight = 1, IdRequestRightGrantType = 1 },
                new { IdAssoc = 5, IdRequestUserAssoc = 3, IdResourceRight = 3, IdRequestRightGrantType = 1 },
                new { IdAssoc = 6, IdRequestUserAssoc = 4, IdResourceRight = 2, IdRequestRightGrantType = 1 },
                new { IdAssoc = 7, IdRequestUserAssoc = 5, IdResourceRight = 1, IdRequestRightGrantType = 1 },
                new { IdAssoc = 8, IdRequestUserAssoc = 5, IdResourceRight = 2, IdRequestRightGrantType = 1 },
                new { IdAssoc = 9, IdRequestUserAssoc = 6, IdResourceRight = 4, IdRequestRightGrantType = 1 },
                new { IdAssoc = 10, IdRequestUserAssoc = 6, IdResourceRight = 6, IdRequestRightGrantType = 1 },
                new { IdAssoc = 11, IdRequestUserAssoc = 8, IdResourceRight = 2, IdRequestRightGrantType = 1 },
                new { IdAssoc = 12, IdRequestUserAssoc = 9, IdResourceRight = 1, IdRequestRightGrantType = 1 },
                new { IdAssoc = 13, IdRequestUserAssoc = 9, IdResourceRight = 2, IdRequestRightGrantType = 1 },
                new { IdAssoc = 14, IdRequestUserAssoc = 10, IdResourceRight = 2, IdRequestRightGrantType = 1 },
                new { IdAssoc = 15, IdRequestUserAssoc = 10, IdResourceRight = 4, IdRequestRightGrantType = 1 },
                new { IdAssoc = 16, IdRequestUserAssoc = 11, IdResourceRight = 3, IdRequestRightGrantType = 1 },
                new { IdAssoc = 17, IdRequestUserAssoc = 11, IdResourceRight = 6, IdRequestRightGrantType = 1 },
                new { IdAssoc = 18, IdRequestUserAssoc = 12, IdResourceRight = 2, IdRequestRightGrantType = 1 },
                new { IdAssoc = 19, IdRequestUserAssoc = 12, IdResourceRight = 6, IdRequestRightGrantType = 1 },
                new { IdAssoc = 20, IdRequestUserAssoc = 12, IdResourceRight = 7, IdRequestRightGrantType = 1 },
                new { IdAssoc = 21, IdRequestUserAssoc = 13, IdResourceRight = 2, IdRequestRightGrantType = 1 },
                new { IdAssoc = 22, IdRequestUserAssoc = 14, IdResourceRight = 5, IdRequestRightGrantType = 1 },
                new { IdAssoc = 23, IdRequestUserAssoc = 16, IdResourceRight = 1, IdRequestRightGrantType = 1 },
                new { IdAssoc = 24, IdRequestUserAssoc = 16, IdResourceRight = 2, IdRequestRightGrantType = 1 },
                new { IdAssoc = 25, IdRequestUserAssoc = 16, IdResourceRight = 3, IdRequestRightGrantType = 1 },
                new { IdAssoc = 26, IdRequestUserAssoc = 17, IdResourceRight = 4, IdRequestRightGrantType = 1 },
                new { IdAssoc = 27, IdRequestUserAssoc = 18, IdResourceRight = 3, IdRequestRightGrantType = 1 },
                new { IdAssoc = 28, IdRequestUserAssoc = 19, IdResourceRight = 3, IdRequestRightGrantType = 1 },
                new { IdAssoc = 29, IdRequestUserAssoc = 19, IdResourceRight = 7, IdRequestRightGrantType = 1 },
                new { IdAssoc = 30, IdRequestUserAssoc = 20, IdResourceRight = 1, IdRequestRightGrantType = 1 },
                new { IdAssoc = 31, IdRequestUserAssoc = 20, IdResourceRight = 4, IdRequestRightGrantType = 1 },
                new { IdAssoc = 32, IdRequestUserAssoc = 21, IdResourceRight = 2, IdRequestRightGrantType = 1 },
                new { IdAssoc = 33, IdRequestUserAssoc = 22, IdResourceRight = 3, IdRequestRightGrantType = 1 },
                new { IdAssoc = 34, IdRequestUserAssoc = 23, IdResourceRight = 4, IdRequestRightGrantType = 1 },
                new { IdAssoc = 35, IdRequestUserAssoc = 24, IdResourceRight = 2, IdRequestRightGrantType = 1 },
                new { IdAssoc = 36, IdRequestUserAssoc = 25, IdResourceRight = 1, IdRequestRightGrantType = 1 },
                new { IdAssoc = 37, IdRequestUserAssoc = 25, IdResourceRight = 2, IdRequestRightGrantType = 1 },
                new { IdAssoc = 38, IdRequestUserAssoc = 26, IdResourceRight = 8, IdRequestRightGrantType = 1 },
                new { IdAssoc = 39, IdRequestUserAssoc = 27, IdResourceRight = 6, IdRequestRightGrantType = 1 },
                new { IdAssoc = 40, IdRequestUserAssoc = 27, IdResourceRight = 7, IdRequestRightGrantType = 1 },
                new { IdAssoc = 41, IdRequestUserAssoc = 28, IdResourceRight = 1, IdRequestRightGrantType = 1 },
                new { IdAssoc = 42, IdRequestUserAssoc = 29, IdResourceRight = 2, IdRequestRightGrantType = 1 },
                new { IdAssoc = 43, IdRequestUserAssoc = 30, IdResourceRight = 2, IdRequestRightGrantType = 1 },
                new { IdAssoc = 44, IdRequestUserAssoc = 30, IdResourceRight = 4, IdRequestRightGrantType = 1 },
                new { IdAssoc = 45, IdRequestUserAssoc = 31, IdResourceRight = 2, IdRequestRightGrantType = 1 },
                new { IdAssoc = 46, IdRequestUserAssoc = 31, IdResourceRight = 3, IdRequestRightGrantType = 1 },
            };
            foreach (var ruarParams in ruars)
            {
                var ruar = new RequestUserRightAssoc
                {
                    IdAssoc = ruarParams.IdAssoc,
                    IdRequestUserAssoc = ruarParams.IdRequestUserAssoc,
                    RequestUserAssoc = RequestUserAssocs.First(r => r.IdRequestUserAssoc == ruarParams.IdRequestUserAssoc),
                    IdResourceRight = ruarParams.IdResourceRight,
                    ResourceRight = ResourceRights.First(r => r.IdResourceRight == ruarParams.IdResourceRight),
                    IdRequestRightGrantType = ruarParams.IdRequestRightGrantType,
                    RequestRightGrantType = RequestRightGrantTypes.First(r => r.IdRequestRightGrantType == ruarParams.IdRequestRightGrantType)
                };
                ResourceRights.First(r => r.IdResourceRight == ruarParams.IdResourceRight).RequestUserRightAssoc.Add(ruar);
                RequestUserAssocs.First(r => r.IdRequestUserAssoc == ruarParams.IdRequestUserAssoc).RequestUserRightAssocs.Add(ruar);
                RequestRightGrantTypes.First(r => r.IdRequestRightGrantType == ruarParams.IdRequestRightGrantType).RequestUserRightAssoc.Add(ruar);
            }
        }

        private void LoadResourceRights()
        {
            var rights = new[]
            {
                new { IdResourceRight = 1, IdResource = 1, Deleted = false },
                new { IdResourceRight = 2, IdResource = 2, Deleted = false },
                new { IdResourceRight = 8, IdResource = 2, Deleted = false },
                new { IdResourceRight = 9, IdResource = 2, Deleted = true },
                new { IdResourceRight = 3, IdResource = 3, Deleted = false },
                new { IdResourceRight = 4, IdResource = 4, Deleted = false },
                new { IdResourceRight = 5, IdResource = 5, Deleted = true },
                new { IdResourceRight = 6, IdResource = 5, Deleted = false },
                new { IdResourceRight = 7, IdResource = 5, Deleted = false },
                new { IdResourceRight = 8, IdResource = 6, Deleted = false },
            };

            foreach (var rightParams in rights)
            {
                var right = new ResourceRight
                {
                    IdResourceRight = rightParams.IdResourceRight,
                    IdResource = rightParams.IdResource,
                    Resource = Resources.First(r => r.IdResource == rightParams.IdResource),
                    Deleted = rightParams.Deleted,
                    RequestUserRightAssoc = new List<RequestUserRightAssoc>()
                };
                ResourceRights.Add(right);
                Resources.First(r => r.IdResource == rightParams.IdResource).ResourceRights.Add(right);
            }
        }

        private void LoadResources()
        {
            var resources = new[]
            {
                new { IdResource = 1, IdResourceGroup = 1, IdDepartment = 24, Deleted = false },
                new { IdResource = 2, IdResourceGroup = 1, IdDepartment = 2, Deleted = false },
                new { IdResource = 3, IdResourceGroup = 2, IdDepartment = 3, Deleted = false },
                new { IdResource = 4, IdResourceGroup = 2, IdDepartment = 3, Deleted = true },
                new { IdResource = 5, IdResourceGroup = 2, IdDepartment = 24, Deleted = false },
                new { IdResource = 6, IdResourceGroup = 2, IdDepartment = 1, Deleted = false },
            };

            foreach (var resourceParams in resources)
            {
                var resource = new Resource
                {
                    IdResource = resourceParams.IdResource,
                    IdResourceGroup = resourceParams.IdResourceGroup,
                    ResourceGroup = ResourceGroups.First(r => r.IdResourceGroup == resourceParams.IdResourceGroup),
                    IdOperatorDepartment = resourceParams.IdDepartment,
                    OperatorDepartment = Departments.First(r => r.IdDepartment == resourceParams.IdDepartment),
                    ResourceRights = new List<ResourceRight>(),
                    Deleted = resourceParams.Deleted
                };
                Resources.Add(resource);
                ResourceGroups.First(r => r.IdResourceGroup == resourceParams.IdResourceGroup).Resources.Add(resource);
                Departments.First(r => r.IdDepartment == resourceParams.IdDepartment).Resources.Add(resource);
            }
        }

        private void LoadResourceGroups()
        {
            var resourceGroups = new[]
            {
                new { IdResourceGroup = 1, Deleted = false },
                new { IdResourceGroup = 2, Deleted = false },
                new { IdResourceGroup = 3, Deleted = true },
            };
            foreach (var resourceGroupParams in resourceGroups)
            {
                ResourceGroups.Add(new ResourceGroup
                {
                    IdResourceGroup = resourceGroupParams.IdResourceGroup,
                    Resources = new List<Resource>(),
                    Deleted = resourceGroupParams.Deleted
                });
            }
        }

        private void LoadAclRoles()
        {
            var roles = new[]
            {
                new { IdRole = 1 },
                new { IdRole = 2 },
                new { IdRole = 3 },
                new { IdRole = 4 },
                new { IdRole = 5 },
                new { IdRole = 6 },
                new { IdRole = 7 },
                new { IdRole = 8 },
            };
            foreach (var roleParams in roles)
            {
                AclRoles.Add(new AclRole { IdRole = roleParams.IdRole, Users = new List<AclUser>() });
            }
        }

        private void LoadDepartments()
        {
            var departments = new[]
            {
                new { IdDepartment = 1, IdParentDepartment = (int?)null },
                new { IdDepartment = 11, IdParentDepartment = (int?)1 },
                new { IdDepartment = 12, IdParentDepartment = (int?)1 },
                new { IdDepartment = 2, IdParentDepartment = (int?)null },
                new { IdDepartment = 21, IdParentDepartment = (int?)2 },
                new { IdDepartment = 22, IdParentDepartment = (int?)2 },
                new { IdDepartment = 3, IdParentDepartment = (int?)null },
                new { IdDepartment = 31, IdParentDepartment = (int?)3 },
                new { IdDepartment = 4, IdParentDepartment = (int?)null },
                new { IdDepartment = 24, IdParentDepartment = (int?)null } // CIT
            };
            foreach (var departmentParams in departments)
            {
                var department = new Department
                {
                    IdDepartment = departmentParams.IdDepartment,
                    IdParentDepartment = departmentParams.IdParentDepartment,
                    AclUsers = new List<AclUser>(),
                    ChildDepartments = new List<Department>(),
                    Users = new List<AclUser>(),
                    Resources = new List<Resource>()
                };
                Departments.Add(department);
                if (departmentParams.IdParentDepartment == null) continue;
                department.ParentDepartment =
                    Departments.First(r => r.IdDepartment == departmentParams.IdParentDepartment.Value);
                department.ParentDepartment.ChildDepartments.Add(department);
            }
        }

        private void LoadUsers()
        {
            var users = new[]
            {
                new { 
                    IdUser = 1,
                    Login = "admin1", 
                    Snp = "admin1",
                    Roles = new List<int> { 1 },
                    IdDepartment = 24,
                    AclDepartments = new List<int>(),
                    Deleted = false
                },
                new { 
                    IdUser = 2,
                    Login = "admin2", 
                    Snp = "admin2",
                    Roles = new List<int> { 1 },
                    IdDepartment = 2,
                    AclDepartments = new List<int>(),
                    Deleted = false
                },
                new { 
                    IdUser = 3,
                    Login = "requester1", 
                    Snp = "requester1",
                    Roles = new List<int> { 3 },
                    IdDepartment = 2,
                    AclDepartments = new List<int>(),
                    Deleted = false
                },
                new { 
                    IdUser = 4,
                    Login = "requester2", 
                    Snp = "requester2", 
                    Roles = new List<int> { 2, 3 },
                    IdDepartment = 3,
                    AclDepartments = new List<int> { 1, 2 },
                    Deleted = false
                },
                new { 
                    IdUser = 5,
                    Login = "requester3", 
                    Snp = "requester3", 
                    Roles = new List<int> { 2, 3, 8 },
                    IdDepartment = 1,
                    AclDepartments = new List<int>(),
                    Deleted = false
                },
                new { 
                    IdUser = 15,
                    Login = "requester4", 
                    Snp = "requester4", 
                    Roles = new List<int> { 3 },
                    IdDepartment = 4,
                    AclDepartments = new List<int>(),
                    Deleted = false
                },
                new { 
                    IdUser = 6,
                    Login = "resourceOwner1", 
                    Snp = "resourceOwner1", 
                    Roles = new List<int> { 2 },
                    IdDepartment = 2,
                    AclDepartments = new List<int>(),
                    Deleted = false
                },
                new { 
                    IdUser = 7,
                    Login = "resourceOwner2", 
                    Snp = "resourceOwner2", 
                    Roles = new List<int> { 2, 8 },
                    IdDepartment = 3,
                    AclDepartments = new List<int>(),
                    Deleted = false
                },
                new { 
                    IdUser = 8,
                    Login = "resourceOwner3", 
                    Snp = "resourceOwner3", 
                    Roles = new List<int> { 2 },
                    IdDepartment = 3,
                    AclDepartments = new List<int> { 1, 2 },
                    Deleted = false
                },
                new { 
                    IdUser = 9,
                    Login = "coordinator1", 
                    Snp = "coordinator1", 
                    Roles = new List<int> { 8 },
                    IdDepartment = 1,
                    AclDepartments = new List<int>(),
                    Deleted = false
                },
                new { 
                    IdUser = 10,
                    Login = "coordinator2", 
                    Snp = "coordinator2", 
                    Roles = new List<int> { 8 },
                    IdDepartment = 24,
                    AclDepartments = new List<int>(),
                    Deleted = true
                },
                new { 
                    IdUser = 11,
                    Login = "dispatcher1", 
                    Snp = "dispatcher1", 
                    Roles = new List<int> { 4 },
                    IdDepartment = 24,
                    AclDepartments = new List<int>(),
                    Deleted = false
                },
                new { 
                    IdUser = 12,
                    Login = "registrar1", 
                    Snp = "registrar1", 
                    Roles = new List<int> { 5 },
                    IdDepartment = 24,
                    AclDepartments = new List<int>(),
                    Deleted = false
                },
                new { 
                    IdUser = 13,
                    Login = "executor1", 
                    Snp = "executor1", 
                    Roles = new List<int> { 6 },
                    IdDepartment = 24,
                    AclDepartments = new List<int>(),
                    Deleted = false
                },
                new { 
                    IdUser = 14,
                    Login = "resourceManager1", 
                    Snp = "resourceManager1", 
                    Roles = new List<int> { 7 },
                    IdDepartment = 24,
                    AclDepartments = new List<int>(),
                    Deleted = false
                }
            };
            foreach (var userParams in users)
            {
                var user = new AclUser
                {
                    IdUser = userParams.IdUser,
                    Login = userParams.Login,
                    Snp = userParams.Snp,
                    IdDepartment = userParams.IdDepartment,
                    Department = Departments.First(r => r.IdDepartment == userParams.IdDepartment),
                    Roles = new List<AclRole>(),
                    AclDepartments = new List<Department>(),
                    Requests = new List<Request>(),
                    RequestUserLastSeens = new List<RequestUserLastSeen>(),
                    RequestAgreements = new List<RequestAgreement>(),
                    RequestsExtComments = new List<RequestExtComment>()
                };
                Departments.First(r => r.IdDepartment == userParams.IdDepartment).Users.Add(user);
                foreach (var roleId in userParams.Roles)
                {
                    var role = AclRoles.First(r => r.IdRole == roleId);
                    user.Roles.Add(role);
                    role.Users.Add(user);
                }
                foreach (var depId in userParams.AclDepartments)
                {
                    var department = Departments.First(r => r.IdDepartment == depId);
                    user.AclDepartments.Add(department);
                    department.AclUsers.Add(user);
                }
                AclUsers.Add(user);
            }
        }

        private void LoadRequestStateTypes()
        {
            var requestStateTypes = new[]
            {
                new { IdRequestStateType = 1, Name = "На согласовании" },
                new { IdRequestStateType = 2, Name = "Утвержденная" },
                new { IdRequestStateType = 3, Name = "На исполнении" },
                new { IdRequestStateType = 4, Name = "Выполненная" },
                new { IdRequestStateType = 5, Name = "Отклоненная" }
            };
            foreach (var requestStateTypeParams in requestStateTypes)
            {
                RequestStateTypes.Add(new RequestStateType
                {
                    IdRequestStateType = requestStateTypeParams.IdRequestStateType,
                    Name = requestStateTypeParams.Name,
                    RequestStates = new List<RequestState>()
                });
            }
        }

        private void LoadRequestTypes()
        {
            var requestTypes = new[]
            {
                new { IdRequestType = 1, Name = "На подключение нового сотрудника" },
                new { IdRequestType = 2, Name = "На изменение прав доступа" },
                new { IdRequestType = 3, Name = "На отключение сотрудника" },
                new { IdRequestType = 4, Name = "На временное делегирование прав" }
            };
            foreach (var requestTypeParams in requestTypes)
            {
                RequestTypes.Add(new RequestType
                {
                    IdRequestType = requestTypeParams.IdRequestType,
                    Name = requestTypeParams.Name,
                    Requests = new List<Request>()
                });
            }
        }

        private void LoadRequests()
        {
            var requests = new[]
            {
                new { IdRequest = 1, IdUser = 3, IdRequestType = 1, Deleted = false },
                new { IdRequest = 2, IdUser = 3, IdRequestType = 2, Deleted = false },
                new { IdRequest = 3, IdUser = 4, IdRequestType = 4, Deleted = false },
                new { IdRequest = 4, IdUser = 4, IdRequestType = 3, Deleted = true },
                new { IdRequest = 5, IdUser = 5, IdRequestType = 1, Deleted = false },
                new { IdRequest = 6, IdUser = 3, IdRequestType = 2, Deleted = false },
                new { IdRequest = 7, IdUser = 3, IdRequestType = 4, Deleted = false },
                new { IdRequest = 8, IdUser = 4, IdRequestType = 3, Deleted = false },
                new { IdRequest = 9, IdUser = 4, IdRequestType = 1, Deleted = false },
                new { IdRequest = 10, IdUser = 5, IdRequestType = 2, Deleted = false },
                new { IdRequest = 11, IdUser = 5, IdRequestType = 4, Deleted = false },
                new { IdRequest = 12, IdUser = 5, IdRequestType = 3, Deleted = false },
                new { IdRequest = 13, IdUser = 4, IdRequestType = 1, Deleted = false },
                new { IdRequest = 14, IdUser = 4, IdRequestType = 2, Deleted = false },
                new { IdRequest = 15, IdUser = 15, IdRequestType = 2, Deleted = false }
            };
            foreach (var requestParams in requests)
            {
                var request = new Request
                {
                    IdRequest = requestParams.IdRequest,
                    IdUser = requestParams.IdUser,
                    User = AclUsers.First(r => r.IdUser == requestParams.IdUser),
                    IdRequestType = requestParams.IdRequestType,
                    RequestType = RequestTypes.First(r => r.IdRequestType == requestParams.IdRequestType),
                    RequestStates = new List<RequestState>(),
                    RequestUserAssoc = new List<RequestUserAssoc>(),
                    RequestAgreements = new List<RequestAgreement>(),
                    RequestUserLastSeens = new List<RequestUserLastSeen>(),
                    Deleted = requestParams.Deleted
                };
                AclUsers.First(r => r.IdUser == requestParams.IdUser).Requests.Add(request);
                RequestTypes.First(r => r.IdRequestType == requestParams.IdRequestType).Requests.Add(request);
                Requests.Add(request);
            }
        }

        private void LoadRequestStates()
        {
            var requestStates = new[]
            {
                new { IdRequestState = 1, IdRequestStateType = 1, IdRequest = 1, 
                    Date = new DateTime(2017, 1, 1, 10, 0, 0), Deleted = false },
                new { IdRequestState = 2, IdRequestStateType = 2, IdRequest = 1, 
                    Date = new DateTime(2017, 1, 1, 10, 0, 1), Deleted = true },
                new { IdRequestState = 3, IdRequestStateType = 2, IdRequest = 2, 
                    Date = new DateTime(2017, 1, 2, 10, 0, 0), Deleted = false },
                new { IdRequestState = 4, IdRequestStateType = 3, IdRequest = 2, 
                    Date = new DateTime(2017, 1, 2, 10, 0, 1), Deleted = false },
                new { IdRequestState = 5, IdRequestStateType = 1, IdRequest = 3, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 6, IdRequestStateType = 2, IdRequest = 3, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 7, IdRequestStateType = 3, IdRequest = 3, 
                    Date = new DateTime(2017, 1, 7, 11, 0, 0), Deleted = false },
                new { IdRequestState = 34, IdRequestStateType = 4, IdRequest = 3, 
                    Date = new DateTime(2017, 1, 7, 11, 0, 0), Deleted = false },
                new { IdRequestState = 8, IdRequestStateType = 2, IdRequest = 4, 
                    Date = new DateTime(2017, 1, 8, 11, 0, 0), Deleted = false },
                new { IdRequestState = 9, IdRequestStateType = 1, IdRequest = 5, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 10, IdRequestStateType = 5, IdRequest = 5, 
                    Date = new DateTime(2017, 1, 6, 11, 0, 0), Deleted = false },
                new { IdRequestState = 11, IdRequestStateType = 1, IdRequest = 6, 
                    Date = new DateTime(2017, 1, 6, 11, 0, 0), Deleted = false },
                new { IdRequestState = 12, IdRequestStateType = 5, IdRequest = 6, 
                    Date = new DateTime(2017, 1, 7, 11, 0, 0), Deleted = false },
                new { IdRequestState = 13, IdRequestStateType = 2, IdRequest = 7, 
                    Date = new DateTime(2017, 1, 8, 11, 0, 0), Deleted = false },
                new { IdRequestState = 14, IdRequestStateType = 5, IdRequest = 7, 
                    Date = new DateTime(2017, 1, 9, 11, 0, 0), Deleted = false },
                new { IdRequestState = 15, IdRequestStateType = 2, IdRequest = 8, 
                    Date = new DateTime(2017, 1, 10, 11, 0, 0), Deleted = false },
                new { IdRequestState = 16, IdRequestStateType = 1, IdRequest = 8, 
                    Date = new DateTime(2017, 1, 11, 11, 0, 0), Deleted = false },
                new { IdRequestState = 17, IdRequestStateType = 1, IdRequest = 9, 
                    Date = new DateTime(2017, 1, 12, 11, 0, 0), Deleted = false },
                new { IdRequestState = 18, IdRequestStateType = 2, IdRequest = 9, 
                    Date = new DateTime(2017, 1, 13, 11, 0, 0), Deleted = false },
                new { IdRequestState = 19, IdRequestStateType = 1, IdRequest = 9, 
                    Date = new DateTime(2017, 1, 14, 11, 0, 0), Deleted = false },
                new { IdRequestState = 20, IdRequestStateType = 2, IdRequest = 9, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 21, IdRequestStateType = 1, IdRequest = 10, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 22, IdRequestStateType = 2, IdRequest = 10, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 23, IdRequestStateType = 1, IdRequest = 10, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 24, IdRequestStateType = 2, IdRequest = 10, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 25, IdRequestStateType = 3, IdRequest = 10, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 26, IdRequestStateType = 1, IdRequest = 11, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 27, IdRequestStateType = 2, IdRequest = 11, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 28, IdRequestStateType = 1, IdRequest = 11, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 29, IdRequestStateType = 5, IdRequest = 11, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 30, IdRequestStateType = 2, IdRequest = 12, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 31, IdRequestStateType = 2, IdRequest = 13, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 32, IdRequestStateType = 3, IdRequest = 13, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 33, IdRequestStateType = 4, IdRequest = 13, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 35, IdRequestStateType = 5, IdRequest = 13, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 36, IdRequestStateType = 1, IdRequest = 14, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 37, IdRequestStateType = 2, IdRequest = 14, 
                    Date = new DateTime(2017, 1, 5, 11, 0, 0), Deleted = false },
                new { IdRequestState = 38, IdRequestStateType = 1, IdRequest = 15, 
                    Date = new DateTime(2016, 1, 5, 11, 0, 0), Deleted = false }
            };

            foreach (var requestStateParams in requestStates)
            {
                var requestState = new RequestState
                {
                    IdRequestState = requestStateParams.IdRequestState,
                    IdRequestStateType = requestStateParams.IdRequestStateType,
                    RequestStateType = RequestStateTypes.First(r => r.IdRequestStateType == requestStateParams.IdRequestStateType),
                    IdRequest = requestStateParams.IdRequest,
                    Request = Requests.First(r => r.IdRequest == requestStateParams.IdRequest),
                    Date = requestStateParams.Date,
                    Deleted = requestStateParams.Deleted
                };
                RequestStates.Add(requestState);
                Requests.First(r => r.IdRequest == requestStateParams.IdRequest).RequestStates.Add(requestState);
                RequestStateTypes.First(r => r.IdRequestStateType == requestStateParams.IdRequestStateType).RequestStates.Add(requestState);
            }
        }

        private void LoadRequestUsers()
        {
            var users = new[]
            {
                new { IdRequestUser = 1, Login = "user1", Deleted = false },
                new { IdRequestUser = 2, Login = "user2", Deleted = false },
                new { IdRequestUser = 3, Login = "user3", Deleted = false },
                new { IdRequestUser = 4, Login = "user4", Deleted = false },
                new { IdRequestUser = 5, Login = "user5", Deleted = false },
                new { IdRequestUser = 6, Login = "user6", Deleted = true },
                new { IdRequestUser = 7, Login = "user6", Deleted = false },
            };
            foreach (var userParams in users)
            {
                var requestUser = new RequestUser
                {
                    IdRequestUser = userParams.IdRequestUser,
                    Login = userParams.Login,
                    Deleted = userParams.Deleted,
                    RequestUserAssoc = new List<RequestUserAssoc>()
                };
                Users.Add(requestUser);
            }
        }

        private void LoadRequestUserAssocs()
        {
            var ruas = new[]
            {
                new { IdRequestUserAssoc = 1, IdRequest = 1, IdRequestUser = 1, Deleted = false },
                new { IdRequestUserAssoc = 2, IdRequest = 1, IdRequestUser = 2, Deleted = false },
                new { IdRequestUserAssoc = 3, IdRequest = 1, IdRequestUser = 3, Deleted = true },
                new { IdRequestUserAssoc = 4, IdRequest = 2, IdRequestUser = 2, Deleted = false },
                new { IdRequestUserAssoc = 5, IdRequest = 2, IdRequestUser = 3, Deleted = false },
                new { IdRequestUserAssoc = 6, IdRequest = 3, IdRequestUser = 4, Deleted = false },
                new { IdRequestUserAssoc = 7, IdRequest = 4, IdRequestUser = 5, Deleted = false },
                new { IdRequestUserAssoc = 8, IdRequest = 5, IdRequestUser = 5, Deleted = false },
                new { IdRequestUserAssoc = 9, IdRequest = 5, IdRequestUser = 7, Deleted = false },
                new { IdRequestUserAssoc = 10, IdRequest = 6, IdRequestUser = 5, Deleted = false },
                new { IdRequestUserAssoc = 11, IdRequest = 6, IdRequestUser = 6, Deleted = false },
                new { IdRequestUserAssoc = 12, IdRequest = 7, IdRequestUser = 1, Deleted = false },
                new { IdRequestUserAssoc = 13, IdRequest = 7, IdRequestUser = 2, Deleted = false },
                new { IdRequestUserAssoc = 14, IdRequest = 7, IdRequestUser = 4, Deleted = false },
                new { IdRequestUserAssoc = 15, IdRequest = 8, IdRequestUser = 6, Deleted = false },
                new { IdRequestUserAssoc = 16, IdRequest = 9, IdRequestUser = 1, Deleted = false },
                new { IdRequestUserAssoc = 17, IdRequest = 9, IdRequestUser = 2, Deleted = false },
                new { IdRequestUserAssoc = 18, IdRequest = 9, IdRequestUser = 3, Deleted = false },
                new { IdRequestUserAssoc = 19, IdRequest = 9, IdRequestUser = 4, Deleted = false },
                new { IdRequestUserAssoc = 20, IdRequest = 9, IdRequestUser = 5, Deleted = false },
                new { IdRequestUserAssoc = 21, IdRequest = 10, IdRequestUser = 4, Deleted = false },
                new { IdRequestUserAssoc = 22, IdRequest = 10, IdRequestUser = 7, Deleted = false },
                new { IdRequestUserAssoc = 23, IdRequest = 11, IdRequestUser = 3, Deleted = false },
                new { IdRequestUserAssoc = 24, IdRequest = 11, IdRequestUser = 4, Deleted = false },
                new { IdRequestUserAssoc = 25, IdRequest = 11, IdRequestUser = 7, Deleted = false },
                new { IdRequestUserAssoc = 26, IdRequest = 11, IdRequestUser = 4, Deleted = false },
                new { IdRequestUserAssoc = 27, IdRequest = 12, IdRequestUser = 5, Deleted = false },
                new { IdRequestUserAssoc = 28, IdRequest = 13, IdRequestUser = 2, Deleted = false },
                new { IdRequestUserAssoc = 29, IdRequest = 13, IdRequestUser = 4, Deleted = false },
                new { IdRequestUserAssoc = 30, IdRequest = 14, IdRequestUser = 1, Deleted = false },
                new { IdRequestUserAssoc = 31, IdRequest = 15, IdRequestUser = 1, Deleted = false },
            };
            foreach (var ruaParams in ruas)
            {
                var assoc = new RequestUserAssoc
                {
                    IdRequestUserAssoc = ruaParams.IdRequestUserAssoc,
                    IdRequest = ruaParams.IdRequest,
                    Request = Requests.First(r => r.IdRequest == ruaParams.IdRequest),
                    IdRequestUser = ruaParams.IdRequestUser,
                    RequestUser = Users.First(r => r.IdRequestUser == ruaParams.IdRequestUser),
                    RequestUserRightAssocs = new List<RequestUserRightAssoc>(),
                    Deleted = ruaParams.Deleted
                };
                RequestUserAssocs.Add(assoc);
                Users.First(r => r.IdRequestUser == ruaParams.IdRequestUser).RequestUserAssoc.Add(assoc);
                Requests.First(r => r.IdRequest == ruaParams.IdRequest).RequestUserAssoc.Add(assoc);
            }
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
