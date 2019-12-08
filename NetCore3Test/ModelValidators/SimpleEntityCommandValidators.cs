using FluentValidation;
using NetCore3Test.Dtos.Commands;

namespace NetCore3Test.ModelValidators
{
    public class SimpleEntityCreateCommandValidators : AbstractValidator<SimpleEntityCreateCommand>
    {
        public SimpleEntityCreateCommandValidators()
        {
            RuleFor(x => x.Name).NotNull().Length(1, 2);
        }
    }

    public class SimpleEntityUpdateCommandValidators : AbstractValidator<SimpleEntityUpdateCommand>
    {
        public SimpleEntityUpdateCommandValidators()
        {
            RuleFor(x => x.Name).NotNull().Length(1, 2);
        }
    }
}
