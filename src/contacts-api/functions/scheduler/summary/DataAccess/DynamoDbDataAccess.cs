using Amazon.DynamoDBv2;
using System.Threading.Tasks;
using System.Threading;
using Amazon.DynamoDBv2.Model;
using Milochau.Contacts.Shared.Entities;
using System.Collections.Generic;
using System.Linq;
using Milochau.Core.Aws.DynamoDB;
using Milochau.Contacts.Shared.DynamoDB;
using Milochau.Contacts.Shared.Entities.Types;

namespace Milochau.Contacts.Scheuler.Summary.DataAccess
{
    public interface IDynamoDbDataAccess
    {
        Task SendSummaryAsync(CancellationToken cancellationToken);
    }

    public class DynamoDbDataAccess : BaseDynamoDbDataAccess, IDynamoDbDataAccess
    {
        public DynamoDbDataAccess(IAmazonDynamoDB amazonDynamoDB) : base(amazonDynamoDB)
        {
        }

        public async Task SendSummaryAsync(CancellationToken cancellationToken)
        {
            var keyConditionExpressionBuilder = new DynamoDbKeyConditionExpressionBuilder();
            keyConditionExpressionBuilder.LessOrEqual.Add(Message.K_Status);

            var dynamoDbResponse = await amazonDynamoDB.QueryAsync(new QueryRequest
            {
                TableName = $"{ConventionsPrefix}-table-{Message.TableNameSuffix}",
                IndexName = Message.Message__Gsi_By_Status_ThenBy_Id.IndexName,
                KeyConditionExpression = keyConditionExpressionBuilder.Build(),
                ExpressionAttributeNames = keyConditionExpressionBuilder.GetExpressionAttributeNames(),
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                    .AppendValue(Message.K_Status, MessageStatus.InProgress)
                    .ToDictionary(x => x.Key, x => x.Value),
                Limit = Message.MaxFetchItems,
            }, cancellationToken);

            var messages = dynamoDbResponse.Items.Take(Message.MaxFetchItems).Select(Message.Message__Gsi_By_Status_ThenBy_Id.ParseFromDynamoDb);
        }
    }
}
