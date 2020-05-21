using System;

namespace CleanTemplate.Application.CrossCuttingConcerns.Logging
{
    public interface ICustomLogging
    {
        string ToLog();
    }
}