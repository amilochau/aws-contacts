using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Milochau.Contacts.Scheuler.Summary.DataAccess;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
namespace Milochau.Contacts.Scheuler.Summary
{
    public class Function
    {
        private static async Task Main()
        {
            Func<string, ILambdaContext, Task> handler = FunctionHandler;
            await LambdaBootstrapBuilder.Create(handler, new SourceGeneratorLambdaJsonSerializer<ApplicationJsonSerializerContext>())
                .Build()
                .RunAsync();
        }

        public static async Task FunctionHandler(string request, ILambdaContext context)
        {
            try
            {
                var cancellationToken = CancellationToken.None;

                using var dynamoDBClient = new AmazonDynamoDBClient();
                var dynamoDbDataAccess = new DynamoDbDataAccess(dynamoDBClient);

                await DoAsync(request, context, dynamoDbDataAccess, cancellationToken);
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Error during test {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public static async Task DoAsync(string request, ILambdaContext context, IDynamoDbDataAccess dynamoDbDataAccess, CancellationToken cancellationToken)
        {
            context.Logger.LogInformation(request);

            await dynamoDbDataAccess.SendSummaryAsync(cancellationToken);
        }
    }

    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonSerializable(typeof(string))]
    public partial class ApplicationJsonSerializerContext : JsonSerializerContext
    {
    }
}