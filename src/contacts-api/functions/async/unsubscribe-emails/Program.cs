using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Milochau.Contacts.Async.UnsubscribeEmails.DataAccess;
using System.Collections.Generic;
using System.Linq;
using Amazon.XRay.Recorder.Handlers.AwsSdk;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
namespace Milochau.Contacts.Async.UnsubscribeEmails
{
    public class Function
    {
        private static async Task Main()
        {
            AWSSDKHandler.RegisterXRayForAllServices();

            Func<FunctionRequest, ILambdaContext, Task> handler = FunctionHandler;
            await LambdaBootstrapBuilder.Create(handler, new SourceGeneratorLambdaJsonSerializer<ApplicationJsonSerializerContext>())
                .Build()
                .RunAsync();
        }

        public static async Task FunctionHandler(FunctionRequest request, ILambdaContext context)
        {
            var cancellationToken = CancellationToken.None;

            using var dynamoDBClient = new AmazonDynamoDBClient();
            var dynamoDbDataAccess = new DynamoDbDataAccess(dynamoDBClient);

            await DoAsync(request, context, dynamoDbDataAccess, cancellationToken);
        }

        public static async Task DoAsync(FunctionRequest request, ILambdaContext context, IDynamoDbDataAccess dynamoDbDataAccess, CancellationToken cancellationToken)
        {
            var validationMessages = ValidateRequest(request);
            if (validationMessages != null && validationMessages.Any())
            {
                throw new Exception();
            }

            await dynamoDbDataAccess.DeleteContactUserAsync(request, cancellationToken);
        }

        private static IEnumerable<string> ValidateRequest(FunctionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.EmailAddress))
            {
                yield return "An email address must be set.";
            }
        }
    }

    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonSerializable(typeof(FunctionRequest))]
    public partial class ApplicationJsonSerializerContext : JsonSerializerContext
    {
    }
}