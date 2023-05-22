using System.Collections.Generic;

namespace Milochau.Contacts.Scheduler.Summary.DataAccess
{
    public class EmailRequest
    {
        public List<EmailRequestRecipient> Tos { get; set; } = new List<EmailRequestRecipient>();
        public string TemplateId { get; } = "contacts-summary";
        public Dictionary<string, string> TemplateData { get; set; } = new Dictionary<string, string>();
    }

    public class EmailRequestRecipient
    {
        public string Address { get; set; } = null!;
    }
}
