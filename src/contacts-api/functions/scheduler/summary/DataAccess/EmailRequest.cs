using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("unsubscribe_url")]
        public string UnsubscribeUrl { get; set; } = "https://";

        [JsonPropertyName("messages")]
        public List<EmailRequestContentMessage> Messages { get; set; } = new List<EmailRequestContentMessage>();
    }

    public class EmailRequestContentMessage
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;

        [JsonPropertyName("status")]
        public string Status { get; set; } = null!;

        [JsonPropertyName("sender_email")]
        public string SenderEmail { get; set; } = null!;

        [JsonPropertyName("sender_name")]
        public string SenderName { get; set; } = null!;

        [JsonPropertyName("message")]
        public string Message { get; set; } = null!;
    }
}
