using Amazon.DynamoDBv2;
using System.Threading.Tasks;
using System.Threading;
using Amazon.DynamoDBv2.Model;
using Milochau.Contacts.Shared.Entities;
using System.Collections.Generic;
using System.Linq;
using Milochau.Core.Aws.DynamoDB;
using Milochau.Contacts.Shared.DynamoDB;

namespace Milochau.Contacts.Http.Messages.Get.DataAccess
{
    public interface IDynamoDbDataAccess
    {
        Task<FunctionResponse> GetMessageAsync(FunctionRequest request, CancellationToken cancellationToken);
    }

    public class DynamoDbDataAccess : BaseDynamoDbDataAccess, IDynamoDbDataAccess
    {
        public DynamoDbDataAccess(IAmazonDynamoDB amazonDynamoDB) : base(amazonDynamoDB)
        {
        }

        public async Task<FunctionResponse> GetMessageAsync(FunctionRequest request, CancellationToken cancellationToken)
        {
            var response = await amazonDynamoDB.GetItemAsync(new GetItemRequest
            {
                TableName = $"{ConventionsPrefix}-table-{Message.TableNameSuffix}",
                Key = new Dictionary<string, AttributeValue>()
                    .Append(Message.K_Id, request.MessageId)
                    .ToDictionary(x => x.Key, x => x.Value),
            }, cancellationToken);

            var message = Message.ParseFromDynamoDb(response.Item);
            return new FunctionResponse
            {
                Id = message.Id,
                Creation = message.Creation,
                Ttl = message.Ttl,
                UserId = message.UserId,
                Status = message.Status,
                Content = message.Content,
                Trackings = message.Tracking,
            };
        }
    }
}
