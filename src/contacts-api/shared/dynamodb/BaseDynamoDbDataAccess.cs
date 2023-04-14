using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Milochau.Core.Aws.Abstractions;
using Milochau.Core.Aws.DynamoDB;
using Milochau.Contacts.Shared.Entities;
using Milochau.Contacts.Shared.Entities.Types;
using Milochau.Contacts.Shared.Entities.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Milochau.Contacts.Shared.DynamoDB
{
    public abstract class BaseDynamoDbDataAccess : BaseDynamoDbDataAccess<BaseAccessResult>
    {
        protected BaseDynamoDbDataAccess(IAmazonDynamoDB amazonDynamoDB) : base(amazonDynamoDB)
        {
        }
    }

    public abstract class BaseDynamoDbDataAccess<TAccessResult> where TAccessResult : BaseAccessResult, new()
    {
        public static string ConventionsPrefix { get; set; } = Environment.GetEnvironmentVariable("CONVENTION__PREFIX")!;

        protected readonly IAmazonDynamoDB amazonDynamoDB;

        public BaseDynamoDbDataAccess(IAmazonDynamoDB amazonDynamoDB)
        {
            this.amazonDynamoDB = amazonDynamoDB;
        }
    }
}
