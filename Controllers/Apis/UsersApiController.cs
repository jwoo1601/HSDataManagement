using HyosungManagement.Data;
using HyosungManagement.Filters;
using HyosungManagement.InputModels;
using HyosungManagement.Logging;
using HyosungManagement.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace HyosungManagement.Controllers.Apis
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [RoleRequirement("Master")]
    [Route("api/users")]
    [ApiController]
    public class UsersApiController : ApiControllerBase
    {
        UserDbContext Context { get; }
        ILogger<HSMUser> Logger { get; }
        UserManager<HSMUser> UserManager { get; }
        RoleManager<HSMRole> RoleManager { get; }
        IStringLocalizer<UsersApiController> Localizer { get; }

        public UsersApiController(
            UserDbContext context,
            ILogger<HSMUser> logger,
            UserManager<HSMUser> userManager,
            RoleManager<HSMRole> roleManager,
            IStringLocalizer<UsersApiController> localizer
        )
        {
            Context = context;
            Logger = logger;
            UserManager = userManager;
            RoleManager = roleManager;
            Localizer = localizer;
        }

        // GET /api/users
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var list = await Context.Users.ToListAsync();

            Logger.LogInformation(
                UsersApiLogEvents.GetAllUsers,
                "Successfully retrieved user list - num: {num}.",
                list.Count
            );

            return Ok(list);
        }


        // GET /api/users/{id}
        [HttpGet("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserByIDAsync(
            string id
        )
        {
            var user = await Context.Users.SingleOrDefaultAsync(u => u.Id.Equals(id));
            if (user == null)
            {
                Logger.LogError(
                    UsersApiLogEvents.UserNotFound,
                    "Failed to retrieve user {ID}: user not found.",
                    id
                );

                return NotFound(Localizer["user.notFound"]);
            }

            Logger.LogInformation(
                UsersApiLogEvents.GetUserByID,
                "Successfully retrieved user {@User}",
                user
            );

            return Ok(user);
        }

        // PATCH /api/users/{id}/activate
        [HttpPatch("{id}/activate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActivateUserAsync(
            string id
        )
        {
            try
            {
                var user = await Context.Users.SingleOrDefaultAsync(u => u.Id.Equals(id));
                if (user == null)
                {
                    Logger.LogError(
                        UsersApiLogEvents.UserNotFound,
                        "Failed to activate user {ID}: user not found.",
                        id
                    );

                    return NotFound(Localizer["user.notFound"]);
                }

                user.IsActive = true;
                Context.Update(user);
                await Context.SaveChangesAsync();

                Logger.LogInformation(
                    UsersApiLogEvents.ActivateUser,
                    "Successfully activated user {ID}",
                    user.Id
                );

                return Ok(user);
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    UsersApiLogEvents.ActivateUserError,
                    e,
                    "Failed to activate user {ID}.",
                    id
                );

                return ServerError(Localizer["user.activateFailure"]);
            }
        }

        // PATCH /api/users/{id}/inactivate
        [HttpPatch("{id}/inactivate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InactivateUserAsync(
            string id
        )
        {
            try
            {
                var user = await Context.Users.SingleOrDefaultAsync(u => u.Id.Equals(id));
                if (user == null)
                {
                    Logger.LogError(
                        UsersApiLogEvents.UserNotFound,
                        "Failed to inactivate user {ID}: user not found.",
                        id
                    );

                    return NotFound(Localizer["user.notFound"]);
                }

                user.IsActive = false;
                Context.Update(user);
                await Context.SaveChangesAsync();

                Logger.LogInformation(
                    UsersApiLogEvents.InactivateUser,
                    "Successfully inactivated user {ID}",
                    user.Id
                );

                return Ok(user);
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    UsersApiLogEvents.InactivateUserError,
                    e,
                    "Failed to inactivate user {ID}.",
                    id
                );

                return ServerError(Localizer["user.inactivateFailure"]);
            }
        }

        // PATCH /api/users/{id}/mark-email-confirmed
        [HttpPatch("{id}/mark-email-confirmed")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MarkEmailConfirmedAsync(
            string id
        )
        {
            try
            {
                var user = await Context.Users.SingleOrDefaultAsync(u => u.Id.Equals(id));
                if (user == null)
                {
                    Logger.LogError(
                        UsersApiLogEvents.UserNotFound,
                        "Failed to mark email confirmed for user {ID}: user not found.",
                        id
                    );

                    return NotFound(Localizer["user.notFound"]);
                }

                if (!await UserManager.IsEmailConfirmedAsync(user))
                {
                    user.EmailConfirmed = true;
                    Context.Update(user);
                    await Context.SaveChangesAsync();

                    Logger.LogInformation(
                        UsersApiLogEvents.MarkEmailConfirmed,
                        "Successfully marked email confirmed for user {ID}",
                        user.Id
                    );

                    return Ok(user);
                }
                else
                {
                    Logger.LogError(
                        UsersApiLogEvents.EmailAlreadyConfirmed,
                        "Failed to mark email confirmed for user {ID}: user has already verified email.",
                        id
                    );

                    return BadRequest(Localizer["user.emailAlreadyConfirmed"]);
                }
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    UsersApiLogEvents.MarkEmailConfirmedError,
                    e,
                    "Failed to mark email confirmed for user {ID}.",
                    id
                );

                return ServerError(Localizer["user.markEmailConfirmedFailure"]);
            }
        }

        // PATCH /api/users/{id}/end-lockout
        [HttpPatch("{id}/end-lockout")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EndLockoutAsync(
            string id
        )
        {
            try
            {
                var user = await Context.Users.SingleOrDefaultAsync(u => u.Id.Equals(id));
                if (user == null)
                {
                    Logger.LogError(
                        UsersApiLogEvents.UserNotFound,
                        "Failed to end lockout of user {ID}: user not found.",
                        id
                    );

                    return NotFound(Localizer["user.notFound"]);
                }

                if (await UserManager.IsLockedOutAsync(user))
                {
                    var result = await UserManager.SetLockoutEndDateAsync(user, null);
                    if (result.Succeeded)
                    {
                        Logger.LogInformation(
                            UsersApiLogEvents.EndLockout,
                            "Successfully ended lockout of user {ID}",
                            user.Id
                        );

                        return Ok(user);
                    }
                    else
                    {
                        Logger.LogError(
                            UsersApiLogEvents.EndLockoutError,
                            "Failed to end lockout of user {ID}: errors - {@Errors}.",
                            id,
                            result.Errors
                        );

                        return ServerError(Localizer["user.endLockoutFailure"]);
                    }
                }
                else
                {
                    Logger.LogError(
                        UsersApiLogEvents.UserNotOnLockout,
                        "Failed to end lockout of user {ID}: user is not on lockout.",
                        id
                    );

                    return BadRequest(Localizer["user.notOnLockout"]);
                }
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    UsersApiLogEvents.EndLockoutError,
                    e,
                    "Failed to end lockout of user {ID}.",
                    id
                );

                return ServerError(Localizer["user.endLockoutFailure"]);
            }
        }

        // PATCH /api/users/{id}/set-role
        [HttpPatch("{id}/set-role")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetUserRoleAsync(
            string id,
            [FromBody] UserSetRoleInputModel inputModel
        )
        {
            var user = await Context.Users.SingleOrDefaultAsync(u => u.Id.Equals(id));
            if (user == null)
            {
                Logger.LogError(
                    UsersApiLogEvents.UserNotFound,
                    "Failed to set role of user {ID} to {Role}: user not found.",
                    id,
                    inputModel.Role
                );

                return NotFound(Localizer["user.notFound"]);
            }

            var role = await RoleManager.FindByIdAsync(inputModel.Role);
            if (role == null)
            {
                Logger.LogError(
                    UsersApiLogEvents.RoleNotFound,
                    "Failed to set role of user {ID} to {Role}: role not found.",
                    id,
                    inputModel.Role
                );

                return NotFound(Localizer["role.notFound"]);
            }

            var userRoles = await UserManager.GetRolesAsync(user);
            if (userRoles.Count != 0)
            {
                await UserManager.RemoveFromRolesAsync(user, userRoles);
            }

            var result = await UserManager.AddToRoleAsync(user, role.Name);
            if (result.Succeeded)
            {
                Logger.LogInformation(
                    UsersApiLogEvents.SetUserRole,
                    "Successfully set role of user {ID} to {Role}",
                    user.Id,
                    inputModel.Role
                );

                return Ok(user);
            }
            else
            {
                Logger.LogError(
                    UsersApiLogEvents.SetUserRoleError,
                    "Failed to set role of user {ID} to {Role}: {@Errors}",
                    id,
                    inputModel.Role,
                    result.Errors
                );

                return ServerError(Localizer["user.setUserRoleFailure"]);
            }
        }

        // DELETE /api/users/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var foundUser = await Context.Users.SingleOrDefaultAsync(c => c.Id == id);
            if (foundUser == null)
            {
                Logger.LogError(
                    UsersApiLogEvents.UserNotFound,
                    "Failed to delete user {ID}: user not found.",
                    id
                );

                return NotFound(Localizer["user.notFound"]);
            }

            try
            {
                await UserManager.DeleteAsync(foundUser);

                Logger.LogInformation(
                    UsersApiLogEvents.DeleteUser,
                    "Successfully deleted user {@User}.",
                    foundUser
                );

                return Ok();
            }
            catch (DbUpdateException e)
            {
                Logger.LogError(
                    UsersApiLogEvents.DeleteUserError,
                    e,
                    "Failed to delete user {ID}.",
                    id
                );

                return ServerError(Localizer["user.deleteFailure"]);
            }
        }
    }
}
