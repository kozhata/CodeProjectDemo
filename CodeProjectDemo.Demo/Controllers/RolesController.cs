using CodeProjectDemo.Demo.Attributes;
using CodeProjectDemo.Demo.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CodeProjectDemo.Demo.Controllers
{
    [CustomAuthorize]
    [RoutePrefix("api/roles")]
    public class RolesController : BaseApiController
    {
        [HttpGet]
        [Route("{id:guid}", Name = "GetRoleByIdRoute")]
        public async Task<IHttpActionResult> GetRole(string Id)
        {
            IdentityRole role = await base.RoleManager.FindByIdAsync(Id);

            if (role != null)
            {
                return Ok(TheModelFactory.Create(role));
            }

            return NotFound();
        }

        [HttpGet]
        [Route("", Name = "GetAllRolesRoute")]
        public IHttpActionResult GetAllRoles()
        {
            List<IdentityRole> roles = base.RoleManager.Roles.ToList();

            return Ok(roles);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IHttpActionResult> Create(CreateRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityRole role = new IdentityRole { Name = model.Name };

            IdentityResult result = await base.RoleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            Uri locationHeader = new Uri(Url.Link("GetRoleByIdRoute", new { id = role.Id }));

            return Created(locationHeader, TheModelFactory.Create(role));
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> DeleteRole(string Id)
        {
            IdentityRole role = await base.RoleManager.FindByIdAsync(Id);

            if (role != null)
            {
                IdentityResult result = await this.RoleManager.DeleteAsync(role);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();
            }

            return NotFound();
        }

        [HttpPost]
        [Route("ManageUsersInRole")]
        public async Task<IHttpActionResult> ManageUsersInRole(UsersInRoleModel model)
        {
            IdentityRole role = await base.RoleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ModelState.AddModelError("", "Role does not exist");
                return BadRequest(ModelState);
            }

            foreach (string user in model.EnrolledUsers)
            {
                CustomIdentityUser appUser = await base.UserManager.FindByIdAsync(user);

                if (appUser == null)
                {
                    ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                    continue;
                }

                if (!base.UserManager.IsInRole(user, role.Name))
                {
                    IdentityResult result = await base.UserManager.AddToRoleAsync(user, role.Name);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", String.Format("User: {0} could not be added to role", user));
                    }

                }
            }

            foreach (string user in model.RemovedUsers)
            {
                var appUser = await base.UserManager.FindByIdAsync(user);

                if (appUser == null)
                {
                    ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                    continue;
                }

                IdentityResult result = await base.UserManager.RemoveFromRoleAsync(user, role.Name);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", String.Format("User: {0} could not be removed from role", user));
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}