using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using HyosungManagement.Data;
using HyosungManagement.InputModels;
using HyosungManagement.Logging;
using HyosungManagement.Models;
using HyosungManagement.Models.Identity;
using HyosungManagement.Options;
using HyosungManagement.Services;
using HyosungManagement.ViewModels;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoreLinq;

namespace HyosungManagement.Controllers
{
    [Route("account")]
    public class AccountController : ViewControllerBase
    {
        UserDbContext Context { get; }
        ILogger<HSMUser> Logger { get; }
        IStringLocalizer<AccountController> Localizer { get; }
        UserManager<HSMUser> UserManager { get; }
        SignInManager<HSMUser> SignInManager { get; }
        IConfiguration Configuration { get; }
        IIdentityServerInteractionService InteractionService { get; }
        IEventService AuthEventService { get; }
        IOptions<AccountOptions> Options { get; }
        IViewRendererService Renderer { get; }
        IBackgroundJobClient JobClient { get; }

        public AccountController(
            UserDbContext context,
            ILogger<HSMUser> logger,
            IStringLocalizer<AccountController> localizer,
            UserManager<HSMUser> userManager,
            SignInManager<HSMUser> signInManager,
            IConfiguration configuration,
            IIdentityServerInteractionService interactionService,
            IEventService authEventService,
            IOptions<AccountOptions> options,
            IViewRendererService renderer,
            IBackgroundJobClient jobClient
        )
        {
            Context = context;
            Logger = logger;
            Localizer = localizer;
            UserManager = userManager;
            SignInManager = signInManager;
            Configuration = configuration;
            InteractionService = interactionService;
            AuthEventService = authEventService;
            Options = options;
            Renderer = renderer;
            JobClient = jobClient;
        }

        [HttpGet("access-denied", Name = "AccessDenied")]
        public IActionResult AccessDenied()
        {
            return Error(
                title: Localizer["accessDenied.error.title"],
                message: Localizer["accessDenied.error.message"]
            );
        }

        // GET /account/login
        [HttpGet("login", Name = "LoginView")]
        public IActionResult Login(
            [FromQuery(Name = "redirect_url")] string redirectUrl
        )
        {
            return View(
                new LoginViewModel
                {
                    RedirectUrl = Url.IsLocalUrl(redirectUrl) ? redirectUrl : "/"
                }
            );
        }

        // GET /account/logout?logout_id={string}
        [HttpGet("logout", Name = "Logout")]
        public async Task<IActionResult> Logout(
            [FromQuery(Name = "logout_id")] string logoutID
        )
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                var logoutContext = await InteractionService.GetLogoutContextAsync(logoutID);
                if (logoutContext?.ShowSignoutPrompt == true)
                {
                    // TODO: show logout prompt view
                }

                await HttpContext.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);
                await AuthEventService.RaiseAsync(
                    new UserLogoutSuccessEvent(
                        User.GetSubjectId(),
                        User.GetDisplayName()
                    )
                );
            }

            // User's not logged in; redirect to home
            return Redirect("~/");
        }

        // GET /account/register
        [HttpGet("register", Name = "RegisterView")]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        // POST /account/login
        [HttpPost("login", Name = "Login")]
        [ProducesResponseType(StatusCodes.Status307TemporaryRedirect)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(
            [FromForm] LoginInputModel inputModel
        )
        {
            var authContext = await InteractionService.GetAuthorizationContextAsync(inputModel.RedirectUrl);

            if (inputModel.IsCancellationRequested)
            {
                if (authContext == null)
                {
                    // No authorization context exists; redirect to home
                    return Redirect("~/");
                }

                await InteractionService.DenyAuthorizationAsync(authContext, IdentityServer4.Models.AuthorizationError.AccessDenied);

                return Redirect(inputModel.RedirectUrl);
            }

            if (ModelState.IsValid)
            {
                var foundUser = await UserManager.FindByNameAsync(inputModel.Username);
                if (foundUser == null)
                {
                    foundUser = await UserManager.FindByEmailAsync(inputModel.Username);
                }

                if (foundUser != null)
                {
                    if (await UserManager.IsLockedOutAsync(foundUser))
                    {
                        ModelState.AddModelError(string.Empty, Localizer["login.lockout"]);
                    }

                    if (ModelState.IsValid)
                    {
                        if (await UserManager.CheckPasswordAsync(foundUser, inputModel.Password))
                        {
                            await AuthEventService.RaiseAsync(
                                new UserLoginSuccessEvent(
                                    username: foundUser.UserName,
                                    subjectId: foundUser.Id,
                                    name: foundUser.UserName,
                                    interactive: true,
                                    clientId: authContext?.Client.ClientId
                                )
                            );

                            AuthenticationProperties props = null;
                            if (Options.Value.AllowRememberLogin && inputModel.AllowRememberLogin)
                            {
                                props = new AuthenticationProperties
                                {
                                    IsPersistent = true,
                                    ExpiresUtc = DateTimeOffset.UtcNow.Add(Options.Value.RememberLoginDuration)
                                };
                            }

                            var authIssuer = new IdentityServerUser(foundUser.Id)
                            {
                                DisplayName = foundUser.UserName
                            };

                            await HttpContext.SignInAsync(authIssuer, props);

                            // No authorization context exists
                            if (authContext == null)
                            {
                                if (Url.IsLocalUrl(inputModel.RedirectUrl))
                                {
                                    return Redirect(inputModel.RedirectUrl);
                                }

                                // Redirect to home as the redirect url is not local
                                return Redirect("~/");
                            }

                            // we can trust the client-provided redirect url since GetAuthorizationContextAsync returned non-null
                            return Redirect(inputModel.RedirectUrl);
                        }

                        var lockoutResult = await UserManager.AccessFailedAsync(foundUser);
                        if (lockoutResult.Succeeded)
                        {
                            Logger.LogWarning(
                                AccountLogEvents.LoginFailure,
                                "User {Username} has failed login with {@InputModel} - attempt: {Attempt}",
                                foundUser.UserName,
                                inputModel,
                                await UserManager.GetAccessFailedCountAsync(foundUser)
                            );
                        }
                    }
                }

                await AuthEventService.RaiseAsync(
                    new UserLoginFailureEvent(
                        username: inputModel.Username,
                        error: "Invalid user credentials",
                        clientId: authContext?.Client.ClientId
                    )
                );

                ModelState.AddModelError(string.Empty, Localizer["login.invalidCredentials"]);
            }

            return View(
                "Login",
                new LoginViewModel
                {
                    DefaultUsername = inputModel.Username,
                    DefaultRememberLogin = inputModel.AllowRememberLogin,
                    RedirectUrl = inputModel.RedirectUrl
                }
            );
        }

        // POST /account/register
        [HttpPost("register", Name = "Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(
            [FromForm] RegisterInputModel inputModel
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
                        AccountLogEvents.SecurityCodeNotRegistered,
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
                            AccountLogEvents.InvalidSecurityCode,
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
                                AccountLogEvents.SendEmailAddressVerificationRequest,
                                "Sent 'Confirm Email Address' email to {To} from {From} with token {Token} for {@User}",
                                emailResult.To,
                                emailResult.From,
                                emailResult.ConfirmationToken,
                                newUser
                            );

                            Logger.LogInformation(
                                AccountLogEvents.Register,
                                "Successfully registered user {@User} from {@InputModel}",
                                newUser,
                                inputModel
                            );

                            //if (!UserManager.Options.SignIn.RequireConfirmedAccount)
                            //{
                            //    Logger.LogInformation(
                            //        AccountsApiLogEvents.SignIn,
                            //        "Signed in user {@User}: sign-in options {@Options}",
                            //        newUser,
                            //        inputModel
                            //    );

                            //    await SignInManager.SignInAsync(newUser, isPersistent: false);
                            //}

                            return Success(
                                new SuccessViewModel
                                {
                                    Title = Localizer["register.success.title"],
                                    Message = Localizer["register.success.message", emailResult.To]
                                }
                            );
                        }

                        result.Errors.ForEach(
                            err => ModelState.AddModelError(err.Code, err.Description)
                        );
                    }
                }

                Logger.LogError(
                    AccountLogEvents.RegisterError,
                    "Failed to register user from input {InputModel}",
                    inputModel
                );
            }
            catch (Exception e)
            {
                Logger.LogError(
                    AccountLogEvents.RegisterError,
                    e,
                    "Failed to register user from input {InputModel}",
                    inputModel
                );
            }

            return View(
                "Register",
                new RegisterViewModel
                {
                    DefaultUsername = inputModel.Username,
                    DefaultName = inputModel.Name,
                    DefaultSecurityCode = inputModel.SecurityCode
                }.SetDefaultEmailAddress(inputModel.EmailAddress)
            );
        }

        // GET /account/confirm-email?confirmation_code={string}
        [HttpGet("confirm-email", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(
            [FromQuery(Name = "confirmation_code")] string confirmationCode
        )
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmationCode));
                var userInstance = await UserManager.GetUserAsync(User);
                if (userInstance.EmailConfirmed)
                {
                    return Error(
                        Localizer["confirmEmail.alreadyConfirmed.title"],
                        Localizer["confirmEmail.alreadyConfirmed.message"]
                    );
                }

                var result = await UserManager.ConfirmEmailAsync(userInstance, decodedToken);
                if (result.Succeeded)
                {
                    if (userInstance.Email == Configuration["Master:DefaultEmailAddress"])
                    {
                        await UserManager.AddToRoleAsync(userInstance, "User");
                    }
                    else
                    {
                        await UserManager.AddToRoleAsync(userInstance, "User");
                    }

                    userInstance.IsActive = true;
                    await UserManager.UpdateAsync(userInstance);

                    Logger.LogInformation(
                        AccountLogEvents.VerifyEmailAddress,
                        "Successfully verified email address {Email} with token {Token} for {@User}",
                        userInstance.Email,
                        decodedToken,
                        userInstance
                    );

                    return Success(
                        new SuccessViewModel
                        {
                            Title = Localizer["confirmEmail.success.title"],
                            Message = Localizer["confirmEmail.success.message", userInstance.Email],
                            EnableAutoRedirection = true
                        }
                    );
                }

                Logger.LogError(
                    AccountLogEvents.VerifyEmailAddressError,
                    "An error occured while verifying email address {Email} for {@User}",
                    userInstance.Email,
                    userInstance
                );

                return Error(
                    new ErrorViewModel
                    {
                        Title = Localizer["confirmEmail.error.title"],
                        Message = Localizer["confirmEmail.error.message", userInstance.Email],
                        AdditionalLinkText = Localizer["confirmEmail.error.linkText"],
                        AdditionalLinkUrl = "/account/confirm-email/resend"
                    }
                );
            }

            return RedirectToRoute(
                "LoginView",
                new
                {
                    redirect_url = $"/account/confirm-email?confirmation_code={confirmationCode}"
                }
            );
        }

        // GET /account/confirm-email/resend
        [HttpGet("confirm-email/resend", Name = "ResendConfirmationEmail")]
        public async Task<IActionResult> ResendConfirmationEmailAsync()
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                var userInstance = await UserManager.GetUserAsync(User);
                if (userInstance.EmailConfirmed)
                {
                    return Error(
                        Localizer["confirmEmail.alreadyConfirmed.title"],
                        Localizer["confirmEmail.alreadyConfirmed.message"]
                    );
                }

                try
                {
                    var emailResult = await SendConfirmationEmailAsync(userInstance);

                    Logger.LogInformation(
                        AccountLogEvents.ResendEmailAddressVerificationRequest,
                        "Resent 'Confirm Email Address' email to {To} from {From} with token {Token} for {@User}",
                        emailResult.To,
                        emailResult.From,
                        emailResult.ConfirmationToken,
                        userInstance
                    );

                    return Success(
                        new SuccessViewModel
                        {
                            Title = Localizer["resendConfirmEmail.success.title"],
                            Message = Localizer["resendConfirmEmail.success.message", userInstance.Email]
                        }
                    );
                }
                catch (Exception e)
                {
                    Logger.LogError(
                        AccountLogEvents.ResendEmailAddressVerificationRequestError,
                        e,
                        "An error occured while resending 'Confirm Email Address' email to {To} for {@User}",
                        userInstance.Email,
                        userInstance
                    );

                    return Error(
                        new ErrorViewModel
                        {
                            Title = Localizer["resendConfirmEmail.error.title"],
                            Message = Localizer["resendConfirmEmail.error.message", userInstance.Email],
                            AdditionalLinkText = Localizer["resendConfirmEmail.error.linkText"],
                            AdditionalLinkUrl = "/account/confirm-email/resend"
                        }
                    );
                }
            }

            return RedirectToRoute(
                "LoginView",
                new
                {
                    redirect_url = "/account/confirm-email/resend"
                }
            );
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