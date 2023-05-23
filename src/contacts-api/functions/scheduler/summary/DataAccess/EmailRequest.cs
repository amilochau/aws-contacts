using Milochau.Contacts.Shared.Entities.Types;
using System.Collections.Generic;

namespace Milochau.Contacts.Scheduler.Summary.DataAccess
{
    public class EmailRequest
    {
        public List<EmailRequestRecipient> Tos { get; set; } = new List<EmailRequestRecipient>();
        public string TemplateId { get; } = "contacts-summary";
        public string? RawTemplateData { get; set; }
    }

    public class EmailRequestRecipient
    {
        public string Address { get; set; } = null!;
    }

    public class EmailRequestContent
    {
        public string UnsubscribeUrl { get; set; } = "https://";
        public List<EmailRequestContentMessage> Messages { get; set; } = new List<EmailRequestContentMessage>();
    }

    public class EmailRequestContentMessage
    {
        public string Id { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string SenderEmail { get; set; } = null!;
        public string SenderName { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
