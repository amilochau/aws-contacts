using System;
using System.IO;
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

                await DoAsync(request, context, dynamoDbDataAccess, cancellationToken);
            }
            catch (Exception ex)
            {
                context.Logger.LogError($"Error during test {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public static async Task DoAsync(Stream request, ILambdaContext context, IDynamoDbDataAccess dynamoDbDataAccess, CancellationToken cancellationToken)
        {
            using var streamReader = new StreamReader(request);
            var content = await streamReader.ReadToEndAsync(cancellationToken);
            context.Logger.LogInformation(content);

            await dynamoDbDataAccess.SendSummaryAsync(cancellationToken);
        }
    }
}