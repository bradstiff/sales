using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Proposing.Domain.Model.ProposalAggregate;
using MediatR;
using System.Threading;
using Proposing.Domain.Model.ProposalAggregate.Payroll;

namespace Proposing.API.Infrastructure.Context
{
    public class ProposingContext : DbContext
    {
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<ProposalCountry> ProposalCountries { get; set; }
        public DbSet<PayrollProduct> PayrollProducts { get; set; }
        public DbSet<PayrollProductCountry> PayrollProductCountries { get; set; }
        public DbSet<HrProduct> HrProducts { get; set; }
        public DbSet<HrProductCountry> HrProductCountries { get; set; }

        private readonly IMediator _mediator;

        private ProposingContext(DbContextOptions<ProposingContext> options) : base(options)
        {
            this.ChangeTracker.Tracked += ChangeTracker_Tracked;
        }

        private void ChangeTracker_Tracked(object sender, Microsoft.EntityFrameworkCore.ChangeTracking.EntityTrackedEventArgs e)
        {
            if (e.FromQuery && e.Entry.Entity is Proposal proposal)
            {

            }
        }

        public ProposingContext(DbContextOptions<ProposingContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProposalConfiguration());
            modelBuilder.ApplyConfiguration(new ProposalCountryConfiguration());

            modelBuilder.ApplyConfiguration(new ProductConfiguration<PayrollProduct>("PayrollProduct", p => p.PayrollProduct));
            modelBuilder.ApplyConfiguration(new ProductCountryConfiguration<PayrollProductCountry, PayrollProduct>("PayrollProductCountry"));

            modelBuilder.ApplyConfiguration(new ProductConfiguration<HrProduct>("HrProduct", p => p.HrProduct));
            modelBuilder.ApplyConfiguration(new ProductCountryConfiguration<HrProductCountry, HrProduct>("HrProductCountry"));
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            return await base.SaveChangesAsync();
        }
    }
}
