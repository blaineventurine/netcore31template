using System;
using Domain.Models;
using Service.Interfaces;

namespace Service.Services.Outputs
{
    public class SimpleEntityOutput : AuditableOutput, IOutput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public SimpleEntityOutput() : base()
        {
        }

        public SimpleEntityOutput(SimpleEntity entity) : base(entity)
        {
            Id = entity.Id;
            Name = entity.Name;
        }
    }
}
