using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HyosungManagement.Services
{
    public enum SendEmailState
    {
        Success,
        Canceled,
        Rejected
    }

    public class SendEmailResult
    {
        public SendEmailState State { get; set; }
        public Exception Cause { get; set; }
    }

    public interface IEmailerService
    {
        Task<SendEmailResult> SendEmailAsync(
            MimeMessage message,
            CancellationToken cancellationToken = default
        );

        Task<SendEmailResult> SendEmailFromHtmlAsync(
            string from,
            IEnumerable<string> to,
            string subject,
            string html,
            CancellationToken cancellationToken = default
        );

        Task<SendEmailResult> SendEmailFromTemplateAsync<TModel>(
            string from,
            IEnumerable<string> to,
            string subject,
            string templateName,
            TModel model = default,
            CancellationToken cancellationToken = default
        );
    }
}
