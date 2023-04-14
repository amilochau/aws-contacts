using Milochau.Contacts.Shared.Entities.ValueTypes;
using System.Collections.Generic;

namespace Milochau.Contacts.Http.Messages.Post
{
    public class FunctionResponse
    {
        public string Id { get; set; } = null!;
        public IList<MessageTracking> Trackings { get; set; } = new List<MessageTracking>();
    }
}
