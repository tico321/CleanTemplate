using System;

namespace CleanTemplate.Infrastructure.Logging
{
    public class Log
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public DateTime Timestamp { get; set; }
        public string Exception { get; set; }
        public string CorrelationId { get; set; }
        public string UserId { get; set; }
        public string Properties { get; set; }
    }
}
