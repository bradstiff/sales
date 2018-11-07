using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Proposing.API.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Proposing.API.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Type commandType;
            if (typeof(TRequest).IsGenericType && typeof(TRequest).GetGenericTypeDefinition() == typeof(CommandWithResourceId<,,>))
            {
                //choosing to violate open-closed principle here
                commandType = typeof(TRequest).GetTypeInfo().GenericTypeArguments[1];
            }
            else
            {
                commandType = typeof(TRequest);
            }
            _logger.LogInformation($"Handling {commandType.Name}: {JsonConvert.SerializeObject(request, Formatting.Indented)}");
            var response = await next();
            _logger.LogInformation($"Handled {commandType.Name} with response: {JsonConvert.SerializeObject(response, Formatting.Indented)}");
            return response;
        }
    }
}
