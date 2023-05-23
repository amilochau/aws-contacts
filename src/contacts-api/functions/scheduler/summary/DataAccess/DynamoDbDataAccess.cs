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

namespace Milochau.Contacts.Scheduler.Summary.DataAccess
{
    public interface IDynamoDbDataAccess
    {
        Task<List<EmailRequestContentMessage>> GetSummaryMessagesAsync(CancellationToken cancellationToken);
        Task<List<EmailRequestRecipient>> GetSummaryRecipientsAsync(CancellationToken cancellationToken);
    }

    public class DynamoDbDataAccess : BaseDynamoDbDataAccess, IDynamoDbDataAccess
    {
        public DynamoDbDataAccess(IAmazonDynamoDB amazonDynamoDB) : base(amazonDynamoDB)
        {
        }

        public async Task<List<EmailRequestContentMessage>> GetSummaryMessagesAsync(CancellationToken cancellationToken)
        {
            var newKeyConditionExpressionBuilder = new DynamoDbKeyConditionExpressionBuilder();
            newKeyConditionExpressionBuilder.Equal.Add(Message.K_Status);

            var newDynamoDbResponseTask = amazonDynamoDB.QueryAsync(new QueryRequest
            {
                TableName = $"{ConventionsPrefix}-table-{Message.TableNameSuffix}",
                IndexName = Message.Message__Gsi_By_Status_ThenBy_Creation.IndexName,
                KeyConditionExpression = newKeyConditionExpressionBuilder.Build(),
                ExpressionAttributeNames = newKeyConditionExpressionBuilder.GetExpressionAttributeNames(),
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                    .AppendValue(Message.K_Status, MessageStatus.New)
                    .ToDictionary(x => x.Key, x => x.Value),
                Limit = Message.MaxFetchItems,
            }, cancellationToken);

            var inProgressKeyConditionExpressionBuilder = new DynamoDbKeyConditionExpressionBuilder();
            inProgressKeyConditionExpressionBuilder.Equal.Add(Message.K_Status);

            var inProgressDynamoDbResponseTask = amazonDynamoDB.QueryAsync(new QueryRequest
            {
                TableName = $"{ConventionsPrefix}-table-{Message.TableNameSuffix}",
                IndexName = Message.Message__Gsi_By_Status_ThenBy_Creation.IndexName,
                KeyConditionExpression = inProgressKeyConditionExpressionBuilder.Build(),
                ExpressionAttributeNames = inProgressKeyConditionExpressionBuilder.GetExpressionAttributeNames(),
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                    .AppendValue(Message.K_Status, MessageStatus.InProgress)
                    .ToDictionary(x => x.Key, x => x.Value),
                Limit = Message.MaxFetchItems,
            }, cancellationToken);

            var newDynamoDbResponse = await newDynamoDbResponseTask;
            var inProgressDynamoDbResponse = await inProgressDynamoDbResponseTask;

            return newDynamoDbResponse.Items.Take(Message.MaxFetchItems).Select(Message.Message__Gsi_By_Status_ThenBy_Creation.ParseFromDynamoDb)
                .Concat(inProgressDynamoDbResponse.Items.Take(Message.MaxFetchItems).Select(Message.Message__Gsi_By_Status_ThenBy_Creation.ParseFromDynamoDb))
                .Select(x => new EmailRequestContentMessage
                {
                    Id = x.Id,
                    Status = x.Status.ToString(),
                    Message = x.Content.Message,
                    SenderEmail = x.Content.SenderEmail,
                    SenderName = x.Content.SenderName,
                })
                .ToList();
        }

        public async Task<List<EmailRequestRecipient>> GetSummaryRecipientsAsync(CancellationToken cancellationToken)
        {
            var keyConditionExpressionBuilder = new DynamoDbKeyConditionExpressionBuilder();
            keyConditionExpressionBuilder.Equal.Add(ContactUser.K_ContactUserType);

            var dynamoDbResponse = await amazonDynamoDB.QueryAsync(new QueryRequest
            {
                TableName = $"{ConventionsPrefix}-table-{ContactUser.TableNameSuffix}",
                KeyConditionExpression = keyConditionExpressionBuilder.Build(),
                ExpressionAttributeNames = keyConditionExpressionBuilder.GetExpressionAttributeNames(),
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                    .AppendValue(ContactUser.K_ContactUserType, ContactUserType.Summary)
                    .ToDictionary(x => x.Key, x => x.Value),
                Limit = ContactUser.MaxFetchItems,
            }, cancellationToken);

            return dynamoDbResponse.Items.Take(ContactUser.MaxFetchItems).Select(ContactUser.ParseFromDynamoDb).Select(x => new EmailRequestRecipient
            {
                Address = x.EmailAddress,
            }).ToList();
        }

        private async Task<IEnumerable<Message.Message__Gsi_By_Status_ThenBy_Creation>> GetMessagesAsync(MessageStatus status, CancellationToken cancellationToken)
        {
            var keyConditionExpressionBuilder = new DynamoDbKeyConditionExpressionBuilder();
            keyConditionExpressionBuilder.Equal.Add(Message.K_Status);

            var dynamoDbResponse = await amazonDynamoDB.QueryAsync(new QueryRequest
            {
                TableName = $"{ConventionsPrefix}-table-{Message.TableNameSuffix}",
                IndexName = Message.Message__Gsi_By_Status_ThenBy_Creation.IndexName,
                KeyConditionExpression = keyConditionExpressionBuilder.Build(),
                ExpressionAttributeNames = keyConditionExpressionBuilder.GetExpressionAttributeNames(),
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                    .AppendValue(Message.K_Status, status)
                    .ToDictionary(x => x.Key, x => x.Value),
                Limit = Message.MaxFetchItems,
            }, cancellationToken);

            return dynamoDbResponse.Items.Take(Message.MaxFetchItems).Select(Message.Message__Gsi_By_Status_ThenBy_Creation.ParseFromDynamoDb);
        }
    }
}
