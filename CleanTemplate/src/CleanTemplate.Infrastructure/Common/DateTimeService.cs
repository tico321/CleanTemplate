using System;
using CleanTemplate.Application.Infrastructure;

namespace CleanTemplate.Infrastructure.Common
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}