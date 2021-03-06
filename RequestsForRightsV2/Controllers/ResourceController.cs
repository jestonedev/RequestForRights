﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RequestsForRights.Domain.Entities;
using RequestsForRights.Web.Infrastructure.Helpers;
using RequestsForRights.Web.Infrastructure.Logging;
using RequestsForRights.Web.Infrastructure.Security.Interfaces;
using RequestsForRights.Web.Infrastructure.Services.Interfaces;
using RequestsForRights.Web.Models.FilterOptions;
using RequestsForRights.Web.Models.Models;
using RequestsForRights.Web.Models.ViewModels;

namespace RequestsForRights.Web.Controllers
{
    public class ResourceController : Controller
    {
        private readonly IResourceService _resourceService;
        private readonly IResourceSecurityService _securityService;
        private readonly ILogger _logger;

        public ResourceController(IResourceService resourceService,
            IResourceSecurityService securityService, ILogger logger)
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
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }
            _logger = logger;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            _logger.Error(filterContext.Exception);
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
            if (resource == null)
            {
                return RedirectToAction("NotFoundError", "Home");
            }
            if (!_securityService.CanUpdate(resource))
            {
                return RedirectToAction("ForbiddenError", "Home");
            }
            ViewData["SecurityService"] = _securityService;
            return View(_resourceService.GetResourceViewModelBy(id));
        }

        [HttpPut]
        public ActionResult Update(ResourceViewModel resourceViewModel, ResourceActFilesModel files, 
            IList<RequestPermissionsDepartmentsModel> requestPermissionsDepartments)
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
            Validate(resourceViewModel, files);
            if (!ModelState.IsValid)
            {
                ViewData["SecurityService"] = _securityService;
                return View(_resourceService.GetResourceViewModelBy(resourceViewModel.Resource, requestPermissionsDepartments));
            }
            try
            {
                _resourceService.UpdateResource(resourceViewModel.Resource, files, requestPermissionsDepartments);
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
            if (resource == null)
            {
                return RedirectToAction("NotFoundError", "Home");
            }
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
        public ActionResult Create(ResourceViewModel resourceViewModel, ResourceActFilesModel files,
            IList<RequestPermissionsDepartmentsModel> requestPermissionsDepartments)
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
            Validate(resourceViewModel, files);
            if (!ModelState.IsValid)
            {
                ViewData["SecurityService"] = _securityService;
                return View(_resourceService.GetResourceViewModelBy(resourceViewModel.Resource, requestPermissionsDepartments));
            }
            try
            {
                _resourceService.InsertResource(resourceViewModel.Resource, files, requestPermissionsDepartments);
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

        private void Validate(ResourceViewModel resourceViewModel, ResourceActFilesModel files)
        {
            if (resourceViewModel.Resource.ResourceRights == null ||
                resourceViewModel.Resource.ResourceRights.Count < 1)
            {
                ModelState.AddModelError(string.Empty,
                    @"Необходимо задать по меньшей мере одно право для ресурса");
            }
            ValidateFiles(files.ResourceAuthorityActs, "Files.ResourceAuthorityActs[{0}]");
            ValidateFiles(files.ResourceUsingActs, "Files.ResourceUsingActs[{0}]");
            ValidateFiles(files.ResourceOperatorActs, "Files.ResourceOperatorActs[{0}]");
            if (files.ResourceOperatorPersons != null)
            {
                for (var i = 0; i < files.ResourceOperatorPersons.Count; i++)
                {
                    ValidateFiles(files.ResourceOperatorPersons[i].Acts,
                        "Files.ResourceOperatorPersons[" + i + "].Acts[{0}]");
                }
            }
            if (files.ResourceOwnerPersons != null)
            {
                for (var i = 0; i < files.ResourceOwnerPersons.Count; i++)
                {
                    ValidateFiles(files.ResourceOwnerPersons[i].Acts,
                        "Files.ResourceOwnerPersons[" + i + "].Acts[{0}]");
                }
            }
        }

        private void ValidateFiles(IList<HttpPostedFileBase> files, string fieldTemplate)
        {
            var deniedExtensions = new[]
            {
                "exe", "dll", "js", "bat", "com", "vbs", "sys"
            };
            if (files == null) return;
            for (var i = 0; i < files.Count; i++)
            {
                if (files[i] == null)
                {
                    continue;
                }
                var fileParts = files[i].FileName.Split('.');
                var fileExtension = "";
                if (fileParts.Length > 1)
                {
                    fileExtension = fileParts[fileParts.Length - 1];
                }
                if (deniedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError(string.Format(fieldTemplate, i),
                        @"Запрещено прикреплять исполняемые файлы");
                }
            }
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