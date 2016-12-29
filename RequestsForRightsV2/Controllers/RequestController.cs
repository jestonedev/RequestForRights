using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RequestsForRightsV2.Infrastructure.Services.Interfaces;

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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RequestsByStatesMenuItems()
        {
            return View(_requestsSerivce.GetNotSeenRequestsByStates());
        }
    }
}