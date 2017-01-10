using System;
using System.Web.Mvc;
using RequestsForRightsV2.Infrastructure.Services.Interfaces;
using RequestsForRightsV2.Models.FilterOptions;

namespace RequestsForRightsV2.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestService _requestsSerivce;
        public RequestController(IRequestService requestsSerivce)
        {
            if (requestsSerivce == null)
            {
                throw new ArgumentNullException("requestsSerivce");
            }
            _requestsSerivce = requestsSerivce;
        }

        //
        // GET: /Requests/
        public ActionResult Index(RequestsFilterOptions requestsFilterOptions)
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RequestsByStatesMenuItems()
        {
            return PartialView(_requestsSerivce.GetNotSeenRequestsByStates());
        }
    }
}