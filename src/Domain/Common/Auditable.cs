using System;
using Domain.Interfaces;

namespace Domain.Common
{
    public class Auditable : IAuditable
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string LastUpdateBy { get; set; }
    }
}
