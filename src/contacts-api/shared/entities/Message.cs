using Amazon.DynamoDBv2.Model;
using Milochau.Core.Aws.DynamoDB;
using Milochau.Contacts.Shared.Entities.ValueTypes;
using Milochau.Contacts.Shared.Entities.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Milochau.Contacts.Shared.Entities
{
    public class Message
    {
        public const string TableNameSuffix = "messages";
        public const int MaxFetchItems = 50;

        public const string K_Id = "id";
        public const string K_Creation = "cd";
        public const string K_Ttl = "ttl";
        public const string K_UserId = "user_id";
        public const string K_Status = "st";
        public const string K_Tracking = "tr";
        public const string K_Content = "co";

        public string Id { get; set; } = null!;

        public DateTimeOffset Creation { get; set; }
        public DateTimeOffset? Ttl { get; set; }

        public string? UserId { get; set; } = null!;
        public MessageStatus Status { get; set; }

        public IList<MessageTracking> Tracking { get; set; } = null!;

        public MessageContent Content { get; set; } = null!;

        public Dictionary<string, AttributeValue> FormatForDynamoDb()
        {
            return new Dictionary<string, AttributeValue>()
                .Append(K_Id, Id)
                .Append(K_Creation, Creation)
                .Append(K_Ttl, Ttl)
                .Append(K_UserId, UserId)
                .Append(K_Status, Status)
                .Append(K_Tracking, Tracking.Select(y => y.FormatForDynamoDb()))
                .Append(K_Content, Content.FormatForDynamoDb())
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public static Message ParseFromDynamoDb(Dictionary<string, AttributeValue?> attributes)
        {
            return new Message
            {
                Id = attributes.ReadString(K_Id),
                Creation = attributes.ReadDateTimeOffset(K_Creation),
                Ttl = attributes.ReadDateTimeOffsetOptional(K_Ttl),
                UserId = attributes.ReadStringOptional(K_UserId),
                Status = attributes.ReadEnumOptional<MessageStatus>(K_Status) ?? MessageStatus.New,
                Tracking = attributes.ReadListString(K_Tracking).Select(MessageTracking.ParseFromDynamoDb).ToList(),
                Content = MessageContent.ParseFromDynamoDb(attributes.ReadObject(K_Content)),
            };
        }
    }
}
