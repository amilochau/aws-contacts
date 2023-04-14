using Milochau.Contacts.Shared.Entities.Types;
using System;
using System.Text;

namespace Milochau.Contacts.Shared.Entities.ValueTypes
{
    public class MessageTracking
    {
        private const char separator = '+';

        public DateTimeOffset Creation { get; set; }
        public string? UserId { get; set; } = null!;
        public MessageTrackingType Type { get; set; }

        public MessageTracking()
        {

        }
        public MessageTracking(string? userId, MessageTrackingType trackingType)
        {
            Creation = DateTimeOffset.Now;
            UserId = userId;
            Type = trackingType;
        }

        public string FormatForDynamoDb()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendJoin(separator, Creation.ToUnixTimeSeconds(), UserId, Convert.ToInt32(Type));
            return stringBuilder.ToString();
        }

        public static MessageTracking ParseFromDynamoDb(string attributeValue)
        {
            var attributes = attributeValue.Split(separator, 3);
            return new MessageTracking
            {
                Creation = DateTimeOffset.FromUnixTimeSeconds(long.Parse(attributes[0])),
                UserId = attributes[1],
                Type = Enum.Parse<MessageTrackingType>(attributes[2]),
            };
        }
    }
}
