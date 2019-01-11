using MediatR;
using Proposing.API.Infrastructure.Context;
using Proposing.API.Domain.Core;
using Proposing.API.Domain.Model.ProposalAggregate;
using Proposing.API.Domain.Model.ProposalAggregate.HR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Proposing.API.Domain.Model.ProposalAggregate.Payroll;

namespace Proposing.API.Application.Commands.UpdateProductScope
{
    public class UpdatePayrollScopeCommand : IRequest<bool>
    {
        public UpdatePayrollScopeCommand()
        {
        }

        public UpdatePayrollScopeCommand(List<UpdatePayrollCountryScopeDto> countryScopes)
        {
            CountryScopes = countryScopes;
        }

        public List<UpdatePayrollCountryScopeDto> CountryScopes { get; set; }
    }

    public class UpdatePayrollCountryScopeDto
    {
        public UpdatePayrollCountryScopeDto()
        {
        }

        public UpdatePayrollCountryScopeDto(int countryId, short levelId, int weeklyPayees, int biWeeklyPayees, int semiMonthlyPayees, int monthlyPayees, bool? reporting, bool? payslipStorage)
        {
            CountryId = countryId;
            LevelId = levelId;
            WeeklyPayees = weeklyPayees;
            BiWeeklyPayees = biWeeklyPayees;
            SemiMonthlyPayees = semiMonthlyPayees;
            MonthlyPayees = monthlyPayees;
            Reporting = reporting;
            PayslipStorage = payslipStorage;
        }

        public int CountryId { get; set; }
        public short LevelId { get; set; }
        public int WeeklyPayees { get; set; }
        public int BiWeeklyPayees { get; set; }
        public int SemiMonthlyPayees { get; set; }
        public int MonthlyPayees { get; set; }
        public bool? Reporting { get; set; }
        public bool? PayslipStorage { get; set; }
    }

    public class UpdatePayrollScopeCommandHandler : IRequestHandler<CommandWithResourceId<int, UpdatePayrollScopeCommand, bool>, bool>
    {
        private readonly ProposingContext _context;
        private readonly IMediator _mediator;

        public UpdatePayrollScopeCommandHandler(ProposingContext context, IMediator mediator)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(CommandWithResourceId<int, UpdatePayrollScopeCommand, bool> request, CancellationToken cancellationToken)
        {
            var command = request.InnerCommand;
            var proposal = await _context.Proposals.FindByIdAsync(request.ResourceId, cancellationToken);

            var productScope = new PayrollScopeDto
            {
                CountryScopes = command.CountryScopes
                    .Where(c => c.LevelId > 0)
                    .Select(c => new PayrollCountryScopeDto
                    {
                        CountryId = c.CountryId,
                        LevelId = c.LevelId,
                        WeeklyPayees = c.WeeklyPayees,
                        BiWeeklyPayees = c.BiWeeklyPayees,
                        SemiMonthlyPayees = c.SemiMonthlyPayees,
                        MonthlyPayees = c.MonthlyPayees,
                        Reporting = c.Reporting,
                        PayslipStorage = c.PayslipStorage
                    })
            };

            proposal.SetProductScope<PayrollScopeDto,PayrollCountryScopeDto>(ProductType.Payroll, productScope);

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
