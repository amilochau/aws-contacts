using Amazon.Lambda.APIGatewayEvents;
using Milochau.Core.Aws.ApiGateway;
using Milochau.Contacts.Shared.Entities.Types;
using Milochau.Contacts.Shared.Entities.ValueTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Milochau.Contacts.Http.Items.Post
{
    public class FunctionRequest : MaybeAuthenticatedRequest, IParsableAndValidatable<FunctionRequest>
    {
        public MessageContent Content { get; set; } = null!;

        public static bool TryParse(APIGatewayHttpApiV2ProxyRequest request, [NotNullWhen(true)] out FunctionRequest? result)
        {
            result = JsonSerializer.Deserialize(request.Body, ApplicationJsonSerializerContext.Default.FunctionRequest);
            if (result == null)
            {
                return false;
            }

            return result.ParseAuthentication(request);
        }

        public void Validate(Dictionary<string, Collection<string>> modelStateDictionary)
        {
            // CONTENT
            modelStateDictionary.ValidateRequired(nameof(Content), Content);

            modelStateDictionary.ValidateRequired(nameof(Content.SenderEmail), Content.SenderEmail);
            modelStateDictionary.ValidateNotWhitespace(nameof(Content.SenderEmail), Content.SenderEmail);
            modelStateDictionary.ValidateMaxLength(nameof(Content.SenderName), Content.SenderName, 100);
            modelStateDictionary.ValidateEmail(nameof(Content.SenderEmail), Content.SenderEmail);

            modelStateDictionary.ValidateRequired(nameof(Content.SenderName), Content.SenderName);
            modelStateDictionary.ValidateNotWhitespace(nameof(Content.SenderName), Content.SenderName);
            modelStateDictionary.ValidateMaxLength(nameof(Content.SenderName), Content.SenderName, 100);

            modelStateDictionary.ValidateRequired(nameof(Content.Message), Content.Message);
            modelStateDictionary.ValidateNotWhitespace(nameof(Content.Message), Content.Message);
            modelStateDictionary.ValidateMaxLength(nameof(Content.Message), Content.Message, 2000);
        }
    }
}