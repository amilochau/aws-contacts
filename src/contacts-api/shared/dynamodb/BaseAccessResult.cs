using Amazon.DynamoDBv2.Model;
using Milochau.Contacts.Shared.Entities.Types;
using System.Collections.Generic;
using System.Linq;

namespace Milochau.Contacts.Shared.DynamoDB
{
    public class BaseAccessResult
    {
        public AccessLevel CurrentUserAccessLevel { get;set; }
        public bool Allowed { get; set; }

        public virtual IEnumerable<string> AdditionalListAttributesToGet => Enumerable.Empty<string>();
        public virtual IEnumerable<string> AdditionalAccessAttributesToGet => Enumerable.Empty<string>();

        public virtual void ParseAccess(Dictionary<string, AttributeValue?> attributes)
        {
        }
        public virtual void ParseList(Dictionary<string, AttributeValue?> attributes)
        {
        }
    }
}
