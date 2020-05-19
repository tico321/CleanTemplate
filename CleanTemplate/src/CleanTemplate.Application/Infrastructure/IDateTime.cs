using System;

namespace CleanTemplate.Application.Infrastructure
{
    // We abstract DateTime because it is a volatile dependency as it is not deterministic.
    public interface IDateTime
    {
        public DateTime Now { get; }
    }
}