using Amazon.DynamoDBv2;
using Amazon.Lambda.TestUtilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Milochau.Core.Aws.Integration;
using System.Collections.Generic;
using System.Threading;

namespace Milochau.Contacts.Tests.Integration.Apis
{
    public static class MessagesApis
    {
        public static IEndpointRouteBuilder MapMessagesApis(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/messages", async (HttpContext httpContext, CancellationToken cancellationToken) =>
            {
                var proxyRequest = await ApiGatewayHelpers.BuildProxyRequestAsync(httpContext, new ProxyRequestOptions(), cancellationToken);
                var dynamoDbDataAccess = new Http.Messages.Post.DataAccess.DynamoDbDataAccess(new AmazonDynamoDBClient());
                var proxyResponse = await Http.Messages.Post.Function.DoAsync(proxyRequest, new TestLambdaContext(), dynamoDbDataAccess, cancellationToken);
                return ApiGatewayHelpers.BuildResult(proxyResponse);
            })
            .Accepts<Http.Messages.Post.FunctionRequest>("application/json")
            .Produces<Http.Messages.Post.FunctionResponse>(200, "application/json")
            .WithTags("Messages")
            .WithSummary("Create a message")
            .WithOpenApi();

            return endpoints;
        }
    }
}
