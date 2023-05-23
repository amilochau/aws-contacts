using Amazon.DynamoDBv2;
using System;

namespace Milochau.Contacts.Shared.DynamoDB
{
    public abstract class BaseDynamoDbDataAccess
    {
        public static string ConventionsPrefix { get; set; } = Environment.GetEnvironmentVariable("CONVENTION__PREFIX")!;

        protected readonly IAmazonDynamoDB amazonDynamoDB;

        public BaseDynamoDbDataAccess(IAmazonDynamoDB amazonDynamoDB)
        {
            this.amazonDynamoDB = amazonDynamoDB;
        }
    }
}
