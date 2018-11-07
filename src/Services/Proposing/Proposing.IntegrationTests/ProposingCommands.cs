using Proposing.API.Application.Commands;
using Proposing.API.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Proposing.IntegrationTests
{
    public class ProposingCommands: IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly ProposingContext _context;

        public ProposingCommands(CustomWebApplicationFactory factory, ProposingContext context)
        {
            _factory = factory;
            _context = context;
        }

        [Fact]
        public async Task CreateProposal_ShouldPass()
        {
            //arrange
            var command = new CreateProposalCommand("Test Proposal", "Test Client", "", null);
            var handler = new CreateProposalCommandHandler(_context);
            
            //act
            var result = await handler.Handle(command, default(CancellationToken));

            //assert
            Assert.InRange(result, 1, int.MaxValue);
        }
    }
}
