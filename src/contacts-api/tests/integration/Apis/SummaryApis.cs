using Amazon.DynamoDBv2;
using Amazon.Lambda;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Milochau.Core.Aws.Integration;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace Milochau.Contacts.Tests.Integration.Apis
{
    public static class SummaryApis
    {
        public static IEndpointRouteBuilder MapSummaryApis(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/summary", async (HttpContext httpContext, CancellationToken cancellationToken) =>
            {
                var dynamoDbDataAccess = new Scheduler.Summary.DataAccess.DynamoDbDataAccess(new AmazonDynamoDBClient());
                var emailsLambdaDataAccess = new Scheduler.Summary.DataAccess.EmailsLambdaDataAccess(new AmazonLambdaClient());
                await Scheduler.Summary.Function.DoAsync(new MemoryStream(), new TestLambdaContext(), dynamoDbDataAccess, emailsLambdaDataAccess, cancellationToken);
                return ApiGatewayHelpers.BuildEmptyResult(new APIGatewayHttpApiV2ProxyResponse
                {
                    StatusCode = 204,
                    Body = string.Empty,
                });
            })
            .Produces(204)
            .WithTags("Summary")
            .WithSummary("Send summary")
            .WithOpenApi();

            endpoints.MapPost("/unsubscribe", async (HttpContext httpContext, CancellationToken cancellationToken) =>
            {
                var proxyRequest = await ApiGatewayHelpers.BuildProxyRequestAsync(httpContext, new ProxyRequestOptions(), cancellationToken);
                var dynamoDbDataAccess = new Async.UnsubscribeEmails.DataAccess.DynamoDbDataAccess(new AmazonDynamoDBClient());
                await Async.UnsubscribeEmails.Function.DoAsync(JsonSerializer.Deserialize<Async.UnsubscribeEmails.FunctionRequest>(proxyRequest.Body, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                })!, new TestLambdaContext(), dynamoDbDataAccess, cancellationToken);
                return ApiGatewayHelpers.BuildEmptyResult(new APIGatewayHttpApiV2ProxyResponse
                {
                    StatusCode = 204,
                    Body = string.Empty,
                });
            })
            .Accepts<Async.UnsubscribeEmails.FunctionRequest>("application/json")
            .Produces(204)
            .WithTags("Summary")
            .WithSummary("Unsubscribe to emails")
            .WithOpenApi();

            return endpoints;
        }
    }
}
