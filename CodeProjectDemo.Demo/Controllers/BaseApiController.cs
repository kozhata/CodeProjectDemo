using CodeProjectDemo.Demo.Infrastructure;
using CodeProjectDemo.Demo.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CodeProjectDemo.Demo.Controllers
{
    public class BaseApiController : ApiController
    {

        private ModelFactory _modelFactory;
        private CustomUserManager _userManager = null;
        private CustomRoleManager _roleManager = null;
        
        protected CustomUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().Get<CustomUserManager>();
            }
        }

        protected CustomRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? Request.GetOwinContext().Get<CustomRoleManager>();
            }
        }

        public BaseApiController()
        {
        }

        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(base.Request, this.UserManager);
                }
                return _modelFactory;
            }
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}