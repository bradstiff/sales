using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Commands
{
    public class CommandWithResourceId<TId, TCommand, TRequest> : IRequest<TRequest>
        where TId : struct
        where TCommand : IRequest<TRequest>
    {
        public CommandWithResourceId()
        {
        }

        public CommandWithResourceId(TId resourceId, TCommand innerCommand)
        {
            ResourceId = resourceId;
            InnerCommand = innerCommand;
        }

        public TId ResourceId { get; private set; }
        public TCommand InnerCommand { get; private set; }
    }
}
