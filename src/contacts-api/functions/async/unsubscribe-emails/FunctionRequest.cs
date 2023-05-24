using Milochau.Contacts.Shared.Entities.Types;

namespace Milochau.Contacts.Async.UnsubscribeEmails
{
    public class FunctionRequest
    {
        public string EmailAddress { get; set; } = null!;
        public EmailSourceType SourceType { get; set; }
    }
}
