using Amazon.DynamoDBv2.Model;
using Milochau.Core.Aws.DynamoDB;
using System.Collections.Generic;
using System.Linq;

namespace Milochau.Contacts.Shared.Entities
{
    public class ContactUser
    {
        public const string TableNameSuffix = "contactusers";
        public const int MaxFetchItems = 50;

        public const string K_ContactUserType = "type";
        public const string K_Id = "id";
        public const string K_EmailAddress = "ea";

        public string ContactUserType { get; set; } = null!;
        public string Id { get; set; } = null!;

        public string EmailAddress { get; set; } = null!;

        public Dictionary<string, AttributeValue> FormatForDynamoDb()
        {
            return new Dictionary<string, AttributeValue>()
                .Append(K_ContactUserType, ContactUserType)
                .Append(K_Id, Id)
                .Append(K_EmailAddress, EmailAddress)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public static ContactUser ParseFromDynamoDb(Dictionary<string, AttributeValue?> attributes)
        {
            return new ContactUser
            {
                ContactUserType = attributes.ReadString(K_ContactUserType),
                Id = attributes.ReadString(K_Id),
                EmailAddress = attributes.ReadString(K_EmailAddress),
            };
        }
    }
}
