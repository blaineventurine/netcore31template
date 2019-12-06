using System;
using Domain.Common;
using Domain.Interfaces;

namespace Domain.Models
{
    public sealed class SimpleEntity : Auditable, IEntity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        private SimpleEntity() { }

        public SimpleEntity(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
