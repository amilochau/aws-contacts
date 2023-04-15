using Amazon.DynamoDBv2;
using System;

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
