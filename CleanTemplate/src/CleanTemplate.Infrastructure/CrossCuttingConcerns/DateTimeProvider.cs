using System;
using CleanTemplate.Application.CrossCuttingConcerns;

namespace CleanTemplate.Infrastructure.CrossCuttingConcerns
{
    public class DateTimeProvider : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
