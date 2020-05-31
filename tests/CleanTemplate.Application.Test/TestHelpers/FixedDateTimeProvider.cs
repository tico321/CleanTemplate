using System;
using CleanTemplate.Application.CrossCuttingConcerns;

namespace CleanTemplate.Application.Test.TestHelpers
{
    public class FixedDateTimeProvider : IDateTime
    {
        public FixedDateTimeProvider(DateTime? dateTime = null)
        {
            Now = dateTime ?? DateTime.Now;
        }

        public DateTime Now { get; }
    }
}
