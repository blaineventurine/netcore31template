using System;

namespace Domain.Interfaces
{
    public interface IAuditable
    {
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime LastUpdatedDate { get; set; }
        string LastUpdateBy { get; set; }
    }
}
