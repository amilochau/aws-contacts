using Milochau.Contacts.Shared.Entities.ValueTypes;
using Milochau.Contacts.Shared.Entities.Types;
using System;
using System.Collections.Generic;

namespace Milochau.Contacts.Http.Messages.Get
{
    public class FunctionResponse
    {
        public string Id { get; set; } = null!;

        public DateTimeOffset Creation { get; set; }
        public DateTimeOffset? Ttl { get; set; }

        public string? UserId { get; set; } = null!;
        public MessageStatus Status { get; set; }

        public MessageContent Content { get; set; } = null!;
        public IList<MessageTracking> Tracking { get; set; } = null!;

    }
}
