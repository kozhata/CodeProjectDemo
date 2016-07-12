using CodeProjectDemo.Demo.Attributes;
using CodeProjectDemo.Demo.Extensions;
using CodeProjectDemo.Demo.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CodeProjectDemo.Demo.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        [CustomAuthorize]
        [HttpGet]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(base.UserManager.Users.ToList().Select(u => base.TheModelFactory.Create(u)));
        }
        
        [CustomAuthorize(Roles ="Admin")]
        [HttpGet]
        [Route("user/{id:guid}", Name = "GetUserByIdRoute")]
        public async Task<IHttpActionResult> GetUser([FromUri]string Id)
        {
            CustomIdentityUser user = await base.UserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(base.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [Authorize]
        [HttpGet]
        [Route("user/{username}", Name ="GetUserByUserNameRoute")]
        public async Task<IHttpActionResult> GetUserByName([FromUri]string username)
        {
            var user = await base.UserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(base.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [HttpPost]
        [Route("create")]
        public async Task<IHttpActionResult> CreateUser([FromBody]CreateUserModel createUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CustomIdentityUser newDbUser = new CustomIdentityUser
            {
                UserName = createUserModel.Username,
                Email = createUserModel.Email,
                FirstName = createUserModel.FirstName,
                LastName = createUserModel.LastName,
                Level = 3,
                JoinDate = DateTime.Now.Date
            };

            IdentityResult addUserResult = await base.UserManager.CreateAsync(newDbUser, createUserModel.Password);

            if (addUserResult.Succeeded)
            {
                string confirmationToken = await base.UserManager.GenerateEmailConfirmationTokenAsync(newDbUser.Id);

                Uri callbackUrl = new Uri(base.Url.Link("ConfirmEmailRoute", new { userId = newDbUser.Id, code = confirmationToken }));

                await base.UserManager.SendEmailAsync(newDbUser.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                Uri locationHeader = new Uri(base.Url.Link("GetUserByIdRoute", new { id = newDbUser.Id }));

                return Created(locationHeader, TheModelFactory.Create(newDbUser));
            }
            else
            {
                return base.GetErrorResult(addUserResult);
            }            
        }

        [HttpGet]
        [Route("confirmemail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail([FromUri]string userId = "", [FromUri]string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.UserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                return base.Ok();
            }
            else
            {
                return base.GetErrorResult(result);
            }
        }

        [HttpPost]
        [Route("changepassword", Name = "ChangePasswordRoute")]
        public async Task<IHttpActionResult> ChangePassword([FromBody]ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await base.UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return base.GetErrorResult(result);
            }

            return Ok();
        }

        [HttpDelete]
        [Route("user/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser([FromUri]string id)
        {

            //Only SuperAdmin or Admin can delete users (Later when implement roles)

            CustomIdentityUser dbUser = await base.UserManager.FindByIdAsync(id);

            if (dbUser != null)
            {
                IdentityResult result = await base.UserManager.DeleteAsync(dbUser);

                if (!result.Succeeded)
                {
                    return base.GetErrorResult(result);
                }

                return Ok();

            }

            return NotFound();

        }

        [CustomAuthorize]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {
            if (!rolesToAssign.Any())
            {
                ModelState.AddModelError("", "Roles to assign array does not exixts");
                return BadRequest(ModelState);
            }

            CustomIdentityUser appUser = await base.UserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                ModelState.AddModelError("", "User not found");
                return NotFound();
            }            

            var rolesNotExists = rolesToAssign.Except(base.RoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Count() > 0)
            {
                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IList<string> currentRoles = await base.UserManager.GetRolesAsync(appUser.Id);

            if (currentRoles.Any())
            {
                IdentityResult removeResult = await this.UserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

                if (!removeResult.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to remove user roles");
                    return BadRequest(ModelState);
                }
            }            

            IdentityResult addResult = await this.UserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [CustomAuthorize(Roles = "Admin")]
        [Route("user/{id:guid}/assignclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignClaimsToUser([FromUri]string id, [FromBody]List<ClaimBindingModel> claimsToAssign)
        {
            if (claimsToAssign == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CustomIdentityUser appUser = await base.UserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel claimModel in claimsToAssign)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {
                    await base.UserManager.RemoveClaimAsync(id, Claims.CreateClaim(claimModel.Type, claimModel.Value));
                }
                
                await base.UserManager.AddClaimAsync(id, Claims.CreateClaim(claimModel.Type, claimModel.Value));
            }

            return Ok();
        }

        [CustomAuthorize(Roles = "Admin")]
        [Route("user/{id:guid}/removeclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> RemoveClaimsFromUser([FromUri] string id, [FromBody] List<ClaimBindingModel> claimsToRemove)
        {
            if (claimsToRemove == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await base.UserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel claimModel in claimsToRemove)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {
                    await base.UserManager.RemoveClaimAsync(id, Claims.CreateClaim(claimModel.Type, claimModel.Value));
                }
            }

            return Ok();
        }
    }
}