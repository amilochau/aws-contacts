using Amazon.DynamoDBv2.Model;
using Milochau.Core.Aws.DynamoDB;
using System.Collections.Generic;
using System.Linq;

namespace Milochau.Contacts.Shared.Entities.ValueTypes
{
    public class MessageContent
    {
        public string SenderEmail { get; set; } = null!;
        public string SenderName { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Culture { get; set; } = null!;

        public Dictionary<string, AttributeValue> FormatForDynamoDb()
        {
            return new Dictionary<string, AttributeValue>()
                .Append("se", SenderEmail)
                .Append("sn", SenderName)
                .Append("m", Message)
                .Append("c", Culture)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public static MessageContent ParseFromDynamoDb(Dictionary<string, AttributeValue?> attributes)
        {
            return new MessageContent
            {
                SenderEmail = attributes.ReadString("se"),
                SenderName = attributes.ReadString("sn"),
                Message = attributes.ReadString("m"),
                Culture = attributes.ReadString("c"),
            };
        }
    }
}
