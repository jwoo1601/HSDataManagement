using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using HyosungManagement.Data;
using HyosungManagement.Filters;
using HyosungManagement.InputModels;
using HyosungManagement.Logging;
using HyosungManagement.Models;
using HyosungManagement.Models.Identity;
using HyosungManagement.Services;
using HyosungManagement.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MimeKit;
using MoreLinq;

namespace HyosungManagement.Controllers.Apis
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [RoleRequirement("Master")]
    [Route("api/accounts")]
    [ApiController]
    public class AccountsApiController : ApiControllerBase
    {
        public static readonly TimeSpan DefaultSecurityCodeAge = TimeSpan.FromDays(1);

        UserDbContext Context { get; }
        ILogger<HSMUser> Logger { get; }
        IStringLocalizer<AccountsApiController> Localizer { get; }
        IConfiguration Configuration { get; }
        UserManager<HSMUser> UserManager { get; }
        IViewRendererService Renderer { get; }
        IBackgroundJobClient JobClient { get; }

        public AccountsApiController(
            UserDbContext context,
            ILogger<HSMUser> logger,
            IStringLocalizer<AccountsApiController> localizer,
            IConfiguration configuration,
            UserManager<HSMUser> userManager,
            IViewRendererService renderer,
            IBackgroundJobClient jobClient
        )
        {
            Context = context;
            Logger = logger;
            Localizer = localizer;
            Configuration = configuration;
            UserManager = userManager;
            Renderer = renderer;
            JobClient = jobClient;
        }

        // GET /api/accounts/security-code
        [HttpGet("security-code")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSecurityCodeListAsync(
            [FromServices] ILogger<SecurityCode> codeLogger
        )
        {
            var list = await Context.SecurityCodes
                                                    .ToListAsync();

            codeLogger.LogInformation(
                AccountsApiLogEvents.GetAllSecurityCodeList,
                "Successfully retrieved security code list - numCode: {numCode}.",
                list.Count
            );

            return Ok(list);
        }

        // POST /api/accounts/security-code/generate
        [HttpPost("security-code/generate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenerateSecurityCodeAsync(
            [FromServices] ILogger<SecurityCode> codeLogger,
            [FromBody] SecurityCodeGenerateInputModel inputModel
        )
        {
            try
            {
                var generatedCode = await inputModel.SaveAsEntityAsync(
                    null,
                    Context,
                    HttpContext.RequestServices
                );

                codeLogger.LogInformation(
                    AccountsApiLogEvents.GenerateSecurityCode,
                    "Successfully generated security code {@Code} with {@InputModel}.",
                    generatedCode,
                    inputModel
                );

                return Created(generatedCode);
            }
            catch (Exception e)
            {
                codeLogger.LogError(
                    AccountsApiLogEvents.GenerateSecurityCodeError,
                    e,
                    "Failed to generate security code with {@InputModel}.",
                    inputModel
                );

                return ServerError(Localizer["securityCode.dbFailure"]);
            }
        }

        // PATCH /api/accounts/security-code/invalidate
        [HttpPatch("security-code/invalidate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InvalidateSecurityCodeAsync(
            [FromServices] ILogger<SecurityCode> codeLogger,
            [FromBody] SecurityCodeInvalidateInputModel inputModel
        )
        {
            try
            {
                var savedCode = await inputModel.SaveAsEntityAsync(null, Context, HttpContext.RequestServices);
                if (savedCode == null)
                {
                    codeLogger.LogError(
                        AccountsApiLogEvents.SecurityCodeNotRegistered,
                        "Failed to invalidate security code {@Code}: no matching code.",
                        inputModel.Code
                    );

                    return NotFound(Localizer["securityCode.notRegistered"]);
                }

                codeLogger.LogInformation(
                    AccountsApiLogEvents.InvalidateSecurityCode,
                    "Successfully invalidated security code {@Code}.",
                    savedCode
                );

                return Ok();
            }
            catch (DbUpdateException e)
            {
                codeLogger.LogError(
                    AccountsApiLogEvents.InvalidateSecurityCodeError,
                    e,
                    "Failed to invalidate security code {@Code}.",
                    inputModel.Code
                );

                return ServerError(Localizer["securityCode.dbFailure"]);
            }
        }

        // POST /api/accounts/register
        [AllowAnonymous]
        [HttpPost("register")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterAsync(
            [FromBody] RegisterInputModel inputModel
        )
        {
            try
            {
                var securityCode = await Context.SecurityCodes
                                                .SingleOrDefaultAsync(
                                                    sc => sc.Value.Equals(inputModel.SecurityCode)
                                                );
                if (securityCode == null)
                {
                    Logger.LogError(
                        AccountsApiLogEvents.SecurityCodeNotRegistered,
                        "Failed to register user from input {@InputModel}: security code {@Code} not registered",
                        inputModel,
                        inputModel.SecurityCode
                    );

                    ModelState.AddModelError("security_code", Localizer["securityCode.notRegistered"]);
                }

                if (ModelState.IsValid)
                {
                    if (!TestSecurityCode(securityCode))
                    {
                        Logger.LogError(
                            AccountsApiLogEvents.InvalidSecurityCode,
                            "Failed to register user from input {@InputModel}: invalid security code {@Code}",
                            inputModel,
                            securityCode
                        );

                        ModelState.AddModelError("security_code", Localizer["securityCode.invalid"]);
                    }


                    if (ModelState.IsValid)
                    {
                        var newUser = new HSMUser
                        {
                            UserName = inputModel.Username,
                            Name = inputModel.Name,
                            Email = inputModel.EmailAddress,
                            SecurityCode = securityCode,
                            Locale = CultureInfo.CurrentUICulture.Name
                        };

                        var result = await UserManager.CreateAsync(newUser, inputModel.Password);
                        if (result.Succeeded)
                        {
                            await UpdateSecurityCodeStatusAsync(securityCode);

                            var emailResult = await SendConfirmationEmailAsync(newUser);

                            Logger.LogInformation(
                                AccountsApiLogEvents.SendEmailAddressVerificationRequest,
                                "Sent 'Confirm Email Address' email to {To} from {From} with token {Token} for {@User}",
                                emailResult.To,
                                emailResult.From,
                                emailResult.ConfirmationToken,
                                newUser
                            );

                            Logger.LogInformation(
                                AccountsApiLogEvents.Register,
                                "Successfully registered user {@User} from {@InputModel}",
                                newUser,
                                inputModel
                            );

                            return Created(newUser);
                        }

                        result.Errors.ForEach(
                            err => ModelState.AddModelError(err.Code, err.Description)
                        );
                    }
                }

                Logger.LogError(
                    AccountsApiLogEvents.RegisterError,
                    "Failed to register user from input {InputModel}",
                    inputModel
                );
            }
            catch (Exception e)
            {
                Logger.LogError(
                    AccountsApiLogEvents.RegisterError,
                    e,
                    "Failed to register user from input {InputModel}",
                    inputModel
                );

                return ServerError(Localizer["register.error"]);
            }

            return ValidationError(ModelState);
        }


        private static bool TestSecurityCode(SecurityCode securityCode)
        {
            if (!securityCode.IsValid)
            {
                return false;
            }

            if (
                securityCode.CodeType != SecurityCodeType.Persistent &&
                securityCode.IsExpired == true
            )
            {
                return false;
            }

            return true;
        }

        private async Task UpdateSecurityCodeStatusAsync(SecurityCode securityCode)
        {
            if (
                securityCode.CodeType == SecurityCodeType.Transient
            )
            {
                securityCode.IsValid = false;
                await Context.SaveChangesAsync();
            }
        }

        private async Task<SendConfirmationEmailResult> SendConfirmationEmailAsync(HSMUser user)
        {
            var from = Configuration["EmailAddrs:NoReply"];
            var to = user.Email;

            var confirmationToken = await UserManager.GenerateEmailConfirmationTokenAsync(
                user
            );
            confirmationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));

            var renderedTemplate = await Renderer.RenderTemplateToStringAsync(
                "ConfirmEmailAddress",
                new ConfirmEmailAddressViewModel
                {
                    Username = user.UserName,
                    ConfirmationCode = confirmationToken
                }
            );

            JobClient.Enqueue<IEmailerService>(
                service => service.SendEmailFromHtmlAsync(
                    from,
                    new[] { to },
                    Localizer["register.confirmEmail"],
                    renderedTemplate,
                    default
                )
            );

            return new SendConfirmationEmailResult
            {
                From = from,
                To = to,
                ConfirmationToken = confirmationToken
            };
        }

        private class SendConfirmationEmailResult
        {
            public string From { get; set; }
            public string To { get; set; }
            public string ConfirmationToken { get; set; }
        }
    }
}
