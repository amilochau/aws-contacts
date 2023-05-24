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

namespace Milochau.Contacts.Async.UnsubscribeEmails.DataAccess
{
    public interface IDynamoDbDataAccess
    {
        Task DeleteContactUserAsync(FunctionRequest request, CancellationToken cancellationToken);
    }

    public class DynamoDbDataAccess : BaseDynamoDbDataAccess, IDynamoDbDataAccess
    {
        public DynamoDbDataAccess(IAmazonDynamoDB amazonDynamoDB) : base(amazonDynamoDB)
        {
        }

        public async Task DeleteContactUserAsync(FunctionRequest request, CancellationToken cancellationToken)
        {
            var keyConditionExpressionBuilder = new DynamoDbKeyConditionExpressionBuilder();
            keyConditionExpressionBuilder.Equal.Add(ContactUser.K_ContactUserType);

            var filterExpressionBuilder = new DynamoDbFilterExpressionBuilder();
            filterExpressionBuilder.Equal.Add(ContactUser.K_EmailAddress);

            var dynamoDbResponse = await amazonDynamoDB.QueryAsync(new QueryRequest
            {
                TableName = $"{ConventionsPrefix}-table-{ContactUser.TableNameSuffix}",
                KeyConditionExpression = keyConditionExpressionBuilder.Build(),
                FilterExpression = filterExpressionBuilder.Build(),
                ExpressionAttributeNames = keyConditionExpressionBuilder.GetExpressionAttributeNames().Union(filterExpressionBuilder.GetExpressionAttributeNames()).ToDictionary(x => x.Key, x => x.Value),
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                    .AppendValue(ContactUser.K_ContactUserType, ContactUserType.Summary)
                    .AppendValue(ContactUser.K_EmailAddress, request.EmailAddress)
                    .ToDictionary(x => x.Key, x => x.Value),
            }, cancellationToken);

            if (!dynamoDbResponse.Items.Any())
            {
                return; // Contact user has not been found
            }

            var contactUser = dynamoDbResponse.Items.Select(ContactUser.ParseFromDynamoDb).First();
            
            await amazonDynamoDB.DeleteItemAsync(new DeleteItemRequest
            {
                TableName = $"{ConventionsPrefix}-table-{ContactUser.TableNameSuffix}",
                Key = new Dictionary<string, AttributeValue>()
                    .Append(ContactUser.K_ContactUserType, ContactUserType.Summary)
                    .Append(ContactUser.K_Id, contactUser.Id)
                    .ToDictionary(x => x.Key, x => x.Value),
            }, cancellationToken);
        }
    }
}
