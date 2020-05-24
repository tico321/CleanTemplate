using System;
using System.Collections.Generic;
using CleanTemplate.Application.CrossCuttingConcerns.JSON;

namespace CleanTemplate.Application.CrossCuttingConcerns.Exceptions
{
    public class AppException : Exception
    {
        public AppException(string message) : base(message)
        {
        }

        public AppException(IEnumerable<string> errors) : base(errors.ToJson())
        {
        }

        public virtual string GetFormattedMessage()
        {
            return Message;
        }
    }
}
