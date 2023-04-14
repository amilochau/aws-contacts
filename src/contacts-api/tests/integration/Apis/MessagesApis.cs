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
            return endpoints;
        }
    }
}
