using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.Lambda;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Runtime.Internal;
using Milochau.Contacts.Scheduler.Summary.DataAccess;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
namespace Milochau.Contacts.Scheduler.Summary
{
    public class Function
    {
        private static async Task Main()
        {
            Func<Stream, ILambdaContext, Task> handler = FunctionHandler;
            await LambdaBootstrapBuilder.Create(handler)
                .Build()
                .RunAsync();
        }

        public static async Task FunctionHandler(Stream request, ILambdaContext context)
        {
            try
            {
                var cancellationToken = CancellationToken.None;

                using var dynamoDBClient = new AmazonDynamoDBClient();
                var dynamoDbDataAccess = new DynamoDbDataAccess(dynamoDBClient);
                using var lambdaClient = new AmazonLambdaClient();
                var emailsLambdaDataAccess = new EmailsLambdaDataAccess(lambdaClient);

                await DoAsync(request, context, dynamoDbDataAccess, emailsLambdaDataAccess, cancellationToken);
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Error during test {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public static async Task DoAsync(Stream request, ILambdaContext context, IDynamoDbDataAccess dynamoDbDataAccess, IEmailsLambdaDataAccess emailsLambdaDataAccess, CancellationToken cancellationToken)
        {
            var summaryMessages = await dynamoDbDataAccess.GetSummaryMessagesAsync(cancellationToken);
            if (!summaryMessages.Any())
            {
                // No pending message, so no need to send summary
                return;
            }

            await emailsLambdaDataAccess.SendSummaryAsync(new EmailRequest
            {
                Tos = new List<EmailRequestRecipient>
                {
                    new EmailRequestRecipient { Address = "" },
                },
                TemplateData = new Dictionary<string, string>
                {
                },
            }, cancellationToken);
        }
    }

    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonSerializable(typeof(EmailRequest))]
    public partial class ApplicationJsonSerializerContext : JsonSerializerContext
    {
    }
}