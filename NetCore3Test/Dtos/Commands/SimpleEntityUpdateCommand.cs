using System;

namespace NetCore3Test.Dtos.Commands
{
    public class SimpleEntityUpdateCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
