using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Domain.Model.ProposalAggregate
{
    public class ComponentType
    {
        public static ComponentType PayrollLevel = new ComponentType(1, "Payroll Level");
        public static ComponentType TimeBundle = new ComponentType(4, "Time Bundle");
        public static ComponentType HrLevel = new ComponentType(10, "HR Level");

        public ComponentType(short id, string name)
        {
            Id = id;
            Name = name;
        }

        public short Id { get; private set; }
        public string Name { get; private set; }

        public override bool Equals(object obj)
        {
            if (!(obj is ComponentType otherValue))
                return false;

            if (!GetType().Equals(obj.GetType()))
                return false;

            if (obj == null)
                return false;

            return this.Id.Equals(otherValue.Id);
        }

        public override int GetHashCode() => this.Id.GetHashCode();
    }
}
