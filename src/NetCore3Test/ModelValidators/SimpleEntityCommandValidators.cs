using FluentValidation;
using NetCore3Test.Dtos.Commands;
using static Common.Constants.NumericalConstants;

namespace NetCore3Test.ModelValidators
{
    public class SimpleEntityCreateCommandValidators : AbstractValidator<SimpleEntityCreateCommand>
    {
        public SimpleEntityCreateCommandValidators()
        {
            RuleFor(x => x.Name).NotNull().Length(MIN_NAME_LENGTH, MAX_NAME_LENGTH);
        }
    }

    public class SimpleEntityUpdateCommandValidators : AbstractValidator<SimpleEntityUpdateCommand>
    {
        public SimpleEntityUpdateCommandValidators()
        {
            RuleFor(x => x.Name).NotNull().Length(MIN_NAME_LENGTH, MAX_NAME_LENGTH);
        }
    }
}
