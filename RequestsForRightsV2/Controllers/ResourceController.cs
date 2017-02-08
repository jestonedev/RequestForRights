using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Web.Mvc;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Infrastructure.Helpers;
using RequestsForRights.Infrastructure.Security.Interfaces;
using RequestsForRights.Infrastructure.Services.Interfaces;
using RequestsForRights.Models.FilterOptions;
using RequestsForRights.Models.Models;
using RequestsForRights.Models.ViewModels;

namespace RequestsForRights.Controllers
{
    public class ResourceController : Controller
    {
        private readonly IResourceService _resourceService;
        private readonly IResourceSecurityService _securityService;

        public ResourceController(IResourceService resourceService,
            IResourceSecurityService securityService)
        {
            if (resourceService == null)
            {
                throw new ArgumentNullException("resourceService");
            }
            _resourceService = resourceService;
            if (securityService == null)
            {
                throw new ArgumentNullException("securityService");
            }
            _securityService = securityService;
        }

        //
        // GET: /ResourceGroup/
        public ActionResult Index(FilterOptions filterOptions)
        {
            if (!_securityService.CanRead())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return View(_resourceService.GetResourceIndexModelView(filterOptions, 
                _resourceService.GetFilteredResources(filterOptions.Filter)));
        }

        public ActionResult GetDataTable(FilterOptions filterOptions)
        {
            if (!_securityService.CanRead())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return PartialView("DataTable", _resourceService.GetResourceIndexModelView(filterOptions,
                _resourceService.GetFilteredResources(filterOptions.Filter)));
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var resource = _resourceService.GetResourceBy(id);
            if (!_securityService.CanUpdate(resource))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return View(_resourceService.GetResourceViewModelBy(id));
        }

        [HttpPut]
        public ActionResult Update(ResourceViewModel resourceViewModel, ResourceActFilesModel files)
        {
            if (resourceViewModel == null || resourceViewModel.Resource == null)
            {
                return RedirectToAction("BadRequestError", "Home",
                    new { message = "Не передана ссылка на ресурс" });
            }
            if (!_securityService.CanUpdate(resourceViewModel.Resource))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            if (resourceViewModel.Resource.ResourceRights == null ||
                resourceViewModel.Resource.ResourceRights.Count < 1)
            {
                ModelState.AddModelError(string.Empty, 
                    @"Необходимо задать по меньшей мере одно право для ресурса");
            }
            if (!ModelState.IsValid)
            {
                ViewData["SecurityService"] = _securityService;
                return View(_resourceService.GetResourceViewModelBy(resourceViewModel.Resource));
            }
            try
            {
                _resourceService.UpdateResource(resourceViewModel.Resource, files);
                _resourceService.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home", 
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
            catch (RetryLimitExceededException e)
            {
                return RedirectToAction("ServerError", "Home",
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
            catch (IOException e)
            {
                return RedirectToAction("ServerError", "Home",
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
            return Request["returnUri"] != null ? (ActionResult)Redirect(Request["returnUri"]) :
                RedirectToAction("Index");
        }

        public ActionResult Detail(int id)
        {
            var resource = _resourceService.GetResourceBy(id);
            if (!_securityService.CanRead(resource))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return View(_resourceService.GetResourceBy(id));
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var resource = _resourceService.GetResourceBy(id);
            if (!_securityService.CanDelete(resource))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            try
            {
                _resourceService.DeleteResource(id);
                _resourceService.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home",
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
            return RedirectToAction("GetDataTable");
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!_securityService.CanCreate())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return View(_resourceService.GetEmptyResourceViewModel());
        }

        [HttpPost]
        public ActionResult Create(ResourceViewModel resourceViewModel, ResourceActFilesModel files)
        {
            if (resourceViewModel == null || resourceViewModel.Resource == null)
            {
                return RedirectToAction("BadRequestError", "Home",
                    new { message = "Не передана ссылка на ресурс" });
            }
            if (!_securityService.CanCreate(resourceViewModel.Resource))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            if (resourceViewModel.Resource.ResourceRights == null || 
                resourceViewModel.Resource.ResourceRights.Count < 1)
            {
                ModelState.AddModelError(string.Empty, 
                    @"Необходимо задать по меньшей мере одно право для ресурса");
            }
            if (!ModelState.IsValid)
            {
                ViewData["SecurityService"] = _securityService;
                return View(_resourceService.GetResourceViewModelBy(resourceViewModel.Resource));
            }
            try
            {
                _resourceService.InsertResource(resourceViewModel.Resource, files);
                _resourceService.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("ConflictError", "Home",
                    new {message = ExceptionHelper.RollToInnerException(e).Message});
            }
            catch (RetryLimitExceededException e)
            {
                return RedirectToAction("ServerError", "Home",
                    new { message = ExceptionHelper.RollToInnerException(e).Message });
            }
            catch (IOException e)
            {
                return RedirectToAction("ServerError", "Home",
                    new {message = ExceptionHelper.RollToInnerException(e).Message});
            }
            return RedirectToAction("Index");
        }

        public ActionResult GetEmptyRightTemplate()
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["RightIndex"] = 0;
            return PartialView("RightEditor", _resourceService.GetEmptyResourceViewModel());
        }

        public ActionResult GetEmptyInternetAddressTemplate()
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["InternetAddressIndex"] = 0;
            var emtpyViewModel = _resourceService.GetEmptyResourceViewModel();
            emtpyViewModel.Resource.ResourceInternetAddresses = new List<ResourceInternetAddress>
            {
                new ResourceInternetAddress()
            };
            return PartialView("InternetAddressEditor", emtpyViewModel);
        }

        public ActionResult GetEmptyDeviceAddressTemplate()
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["DeviceAddressIndex"] = 0;
            var emtpyViewModel = _resourceService.GetEmptyResourceViewModel();
            emtpyViewModel.Resource.ResourceDeviceAddresses = new List<ResourceDeviceAddress>
            {
                new ResourceDeviceAddress()
            };
            return PartialView("DeviceAddressEditor", emtpyViewModel);
        }

        public ActionResult GetEmptyOwnerPersonTemplate()
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["OwnerPersonIndex"] = 0;
            var emtpyViewModel = _resourceService.GetEmptyResourceViewModel();
            emtpyViewModel.Resource.ResourceOwnerPersons = new List<ResourceOwnerPerson>
            {
                new ResourceOwnerPerson()
            };
            return PartialView("OwnerPersonEditor", emtpyViewModel);
        }

        public ActionResult GetEmptyOwnerPersonActTemplate()
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["OwnerPersonIndex"] = 0;
            ViewData["OwnerPersonActIndex"] = 0;
            var emtpyViewModel = _resourceService.GetEmptyResourceViewModel();
            emtpyViewModel.Resource.ResourceOwnerPersons = new List<ResourceOwnerPerson>
            {
                new ResourceOwnerPerson
                {
                    Acts = new List<ResourceOwnerPersonAct>
                    {
                        new ResourceOwnerPersonAct
                        {
                            ActDate = DateTime.Now.Date
                        }
                    }
                }
            };
            return PartialView("OwnerPersonActEditor", emtpyViewModel);
        }

        public ActionResult GetEmptyOperatorPersonTemplate()
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["OperatorPersonIndex"] = 0;
            var emtpyViewModel = _resourceService.GetEmptyResourceViewModel();
            emtpyViewModel.Resource.ResourceOperatorPersons = new List<ResourceOperatorPerson>
            {
                new ResourceOperatorPerson()
            };
            return PartialView("OperatorPersonEditor", emtpyViewModel);
        }

        public ActionResult GetEmptyOperatorPersonActTemplate()
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["OperatorPersonIndex"] = 0;
            ViewData["OperatorPersonActIndex"] = 0;
            var emtpyViewModel = _resourceService.GetEmptyResourceViewModel();
            emtpyViewModel.Resource.ResourceOperatorPersons = new List<ResourceOperatorPerson>
            {
                new ResourceOperatorPerson
                {
                    Acts = new List<ResourceOperatorPersonAct>
                    {
                        new ResourceOperatorPersonAct
                        {
                            ActDate = DateTime.Now.Date
                        }
                    }
                }
            };
            return PartialView("OperatorPersonActEditor", emtpyViewModel);
        }

        public ActionResult GetEmptyOperatorActTemplate()
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["OperatorActIndex"] = 0;
            var emtpyViewModel = _resourceService.GetEmptyResourceViewModel();
            emtpyViewModel.Resource.ResourceOperatorActs = new List<ResourceOperatorAct>
            {
                new ResourceOperatorAct
                {
                    ActDate = DateTime.Now.Date
                }
            };
            return PartialView("OperatorActEditor", emtpyViewModel);
        }

        public ActionResult GetEmptyAuthorityActTemplate()
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["AuthorityActIndex"] = 0;
            var emtpyViewModel = _resourceService.GetEmptyResourceViewModel();
            emtpyViewModel.Resource.ResourceAuthorityActs = new List<ResourceAuthorityAct>
            {
                new ResourceAuthorityAct
                {
                    ActDate = DateTime.Now.Date
                }
            };
            return PartialView("AuthorityActEditor", emtpyViewModel);
        }

        public ActionResult GetEmptyUsingActTemplate()
        {
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["UsingActIndex"] = 0;
            var emtpyViewModel = _resourceService.GetEmptyResourceViewModel();
            emtpyViewModel.Resource.ResourceUsingActs = new List<ResourceUsingAct>
            {
                new ResourceUsingAct
                {
                    ActDate = DateTime.Now.Date
                }
            };
            return PartialView("UsingActEditor", emtpyViewModel);
        }

        public ActionResult LoadFile(int idFile)
        {
            if (!_securityService.CanRead())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            var file = _resourceService.GetActFile(idFile);
            return File(file.FileContent, file.FileContentType, file.FileOriginalName);
        }

        public ActionResult GetDepartmentInfo(int idDepartment)
        {
            if (!_securityService.CanRead())
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            return Json(_resourceService.GetDepartmentInfo(idDepartment), JsonRequestBehavior.AllowGet);
        }
    }
}