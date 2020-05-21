using System;
using CleanTemplate.Application.CrossCuttingConcerns;

namespace CleanTemplate.Infrastructure.CrossCuttingConcerns
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}