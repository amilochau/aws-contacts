using Amazon.DynamoDBv2;
using Milochau.Contacts.Shared.Entities.ValueTypes;
using System.Threading.Tasks;
using System.Threading;
using Milochau.Contacts.Shared.DynamoDB;
using Amazon.DynamoDBv2.Model;
using Milochau.Contacts.Shared.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
using Milochau.Core.Aws.DynamoDB;
using Milochau.Core.Aws.Abstractions;
using System.Globalization;
using Milochau.Contacts.Shared.Entities.Types;

namespace Milochau.Contacts.Http.Items.Post.DataAccess
{
    public interface IDynamoDbDataAccess
    {
        Task<FunctionResponse> CreateMessageAsync(FunctionRequest request, CancellationToken cancellationToken);
    }

    public class DynamoDbDataAccess : BaseDynamoDbDataAccess<BaseAccessResult>, IDynamoDbDataAccess
    {
        public DynamoDbDataAccess(IAmazonDynamoDB amazonDynamoDB) : base(amazonDynamoDB)
        {
        }

        public async Task<FunctionResponse> CreateMessageAsync(FunctionRequest request, CancellationToken cancellationToken)
        {
            var messageTracking = new List<MessageTracking> { new MessageTracking(request.User?.Id, MessageTrackingType.Create) };
            var message = new Message
            {
                Id = Guid.NewGuid().ToString("N"),
                Creation = DateTimeOffset.Now,
                UserId = request.User?.Id,
                Status = MessageStatus.New,
                Tracking = messageTracking,
                Content = request.Content,
            };

            await amazonDynamoDB.PutItemAsync(new PutItemRequest
            {
                TableName = $"{ConventionsPrefix}-table-{Message.TableNameSuffix}",
                Item = message.FormatForDynamoDb(),
            }, cancellationToken);

            return new FunctionResponse
            {
                Id = message.Id,
                Trackings = messageTracking,
            };
        }
    }
}
