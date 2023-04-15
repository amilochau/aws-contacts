using Amazon.Lambda.APIGatewayEvents;
using Milochau.Core.Aws.ApiGateway;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Milochau.Contacts.Http.Messages.Get
{
	public class FunctionRequest : MaybeAuthenticatedRequest, IParsableAndValidatable<FunctionRequest>
    {
        [JsonIgnore]
        public string MessageId { get; set; } = null!;

        public static bool TryParse(APIGatewayHttpApiV2ProxyRequest request, [NotNullWhen(true)] out FunctionRequest? result)
        {
            result = null;

            if (request.PathParameters == null || !request.PathParameters.TryGetValue("messageId", out var messageId) || !Guid.TryParseExact(messageId, "N", out _))
            {
                return false;
            }

            result = new FunctionRequest
            {
                MessageId = messageId
            };

            return result.ParseAuthentication(request);
        }

        public void Validate(Dictionary<string, Collection<string>> modelStateDictionary)
        {
        }
    }
}
