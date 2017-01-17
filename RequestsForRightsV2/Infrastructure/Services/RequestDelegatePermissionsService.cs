using System;
using System.Linq;
using RequestsForRights.Database.Repositories.Interfaces;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Models.Models;

namespace RequestsForRights.Infrastructure.Services
{
    public class RequestDelegatePermissionsService: RequestService<RequestDelegatePermissionsUserModel>
    {
        public RequestDelegatePermissionsService(IRequestRepository requestsRepository, 
            IRequestSecurityService<RequestDelegatePermissionsUserModel> requestSecurityService) : 
            base(requestsRepository, requestSecurityService)
        {
        }

        public override RequestModel<RequestDelegatePermissionsUserModel> GetRequestModelBy(Request request)
        {
            return new RequestModel<RequestDelegatePermissionsUserModel>
            {
                IdRequest = request.IdRequest,
                Description = request.Description,
                IdRequestType = request.IdRequestType,
                Users = request.RequestUserAssoc.Where(ru => !ru.Deleted).Select(ru =>
                {
                    var delegationExtInfo =
                        RequestsRepository.GetDelegationRequestUserExtInfoBy(ru.IdRequestUserAssoc);
                    if (delegationExtInfo == null)
                    {
                        throw new ApplicationException("Отсутствует дополнительная информация о делегировании");
                    }
                    var userModel = FillRequestUserModel(ru);
                    userModel.DelegationRequestUsersExtInfoModel = new DelegationRequestUsersExtInfoModel
                    {
                        LoginDelegateTo = delegationExtInfo.DelegateToUser.Login,
                        PostDelegateTo = delegationExtInfo.DelegateToUser.Post,
                        SnpDelegateTo = delegationExtInfo.DelegateToUser.Snp,
                        PhoneDelegateTo = delegationExtInfo.DelegateToUser.Phone,
                        DepartmentDelegateTo = delegationExtInfo.DelegateToUser.Department,
                        UnitDelegateTo = delegationExtInfo.DelegateToUser.Unit,
                        OfficeDelegateTo = delegationExtInfo.DelegateToUser.Office,
                        DelegateFromDate = delegationExtInfo.DelegateFromDate,
                        DelegateToDate = delegationExtInfo.DelegateToDate
                    };
                    return userModel;
                }).ToList()
            };
        }

        public override Request UpdateRequest(RequestModel<RequestDelegatePermissionsUserModel> requestModel)
        {
            return null;
        }

        public override Request CreateRequest(RequestModel<RequestDelegatePermissionsUserModel> requestModel)
        {
            return null;
        }
    }
}