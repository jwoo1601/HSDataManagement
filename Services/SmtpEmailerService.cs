using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HyosungManagement.Services
{
    public class EmailerOptions
    {
        public static readonly string Emailer = "Emailer";

        public string Host { get; set; }
        public int Port { get; set; }
        public string SecureSocketMode { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SmtpEmailerService : IEmailerService
    {
        IOptions<EmailerOptions> Options { get; }
        IViewRendererService ViewRendererService { get; }

        public SmtpEmailerService(
            IOptions<EmailerOptions> options,
            IViewRendererService viewRendererService
        )
        {
            Options = options;
            ViewRendererService = viewRendererService;
        }

        public async Task<SendEmailResult> SendEmailAsync(
            MimeMessage message,
            CancellationToken cancellationToken = default
        )
        {
            var options = Options.Value;
            SecureSocketOptions secureSocketOption;
            if (!Enum.TryParse(options.SecureSocketMode, true, out secureSocketOption))
            {
                secureSocketOption = SecureSocketOptions.Auto;
            }

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(
                        host: options.Host,
                        port: options.Port,
                        options: SecureSocketOptions.Auto,
                        cancellationToken
                    ).ConfigureAwait(false);
                    await client.AuthenticateAsync(
                        options.Username,
                        options.Password,
                        cancellationToken
                    ).ConfigureAwait(false);
                    await client.SendAsync(message, cancellationToken).ConfigureAwait(false);
                    await client.DisconnectAsync(true, cancellationToken).ConfigureAwait(false);

                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException e)
            {
                return new SendEmailResult
                {
                    State = SendEmailState.Canceled
                };
            }
            catch (Exception e)
            {
                return new SendEmailResult
                {
                    State = SendEmailState.Rejected,
                    Cause = e
                };
            }

            return new SendEmailResult
            {
                State = SendEmailState.Success
            };
        }

        public async Task<SendEmailResult> SendEmailFromHtmlAsync(
            string from,
            IEnumerable<string> to,
            string subject,
            string html,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var emailMessage = new MimeMessage(
                    new[] { MailboxAddress.Parse(from) },
                    to.Select(addr => MailboxAddress.Parse(addr)),
                    subject,
                    new TextPart(TextFormat.Html)
                    {
                        Text = html
                    }
                );

                return await SendEmailAsync(
                    emailMessage,
                    cancellationToken
                );
            }
            catch (Exception e)
            {
                return new SendEmailResult
                {
                    State = SendEmailState.Rejected,
                    Cause = e
                };
            }
        }

        public async Task<SendEmailResult> SendEmailFromTemplateAsync<TModel>(
            string from,
            IEnumerable<string> to,
            string subject,
            string templateName,
            TModel model = default,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var rendered = await ViewRendererService.RenderTemplateToStringAsync(
                    templateName,
                    model
                );
                if (rendered == null)
                {
                    return new SendEmailResult
                    {
                        State = SendEmailState.Rejected
                    };
                }

                return await SendEmailFromHtmlAsync(
                    from,
                    to,
                    subject,
                    rendered,
                    cancellationToken
                );
            }
            catch (Exception e)
            {
                return new SendEmailResult
                {
                    State = SendEmailState.Rejected,
                    Cause = e
                };
            }
        }
    }
}
