using System;
using Domain.Interfaces;

namespace Service.Services.Outputs
{
    public abstract class AuditableOutput
    {
        public DateTime CreatedDate { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime LastUpdatedDate { get; private set; }
        public string LastUpdateBy { get; private set; }

        protected AuditableOutput()
        {
        }

        protected AuditableOutput(IAuditable auditable)
        {
            if (auditable != null)
            {
                CreatedDate = auditable.CreatedDate;
                CreatedBy = auditable.CreatedBy;
                LastUpdatedDate = auditable.LastUpdatedDate;
                LastUpdateBy = auditable.LastUpdateBy;
            }
        }
    }
}
