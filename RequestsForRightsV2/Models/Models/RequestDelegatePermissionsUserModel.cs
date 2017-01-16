using System;
using System.Collections.Generic;

namespace RequestsForRights.Models.Models
{
    public class RequestDelegatePermissionsUserModel: RequestUserModel
    {
        public DelegationRequestUsersExtInfoModel DelegationRequestUsersExtInfoModel { get; set; }
    }
}