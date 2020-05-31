namespace CleanTemplate.Application.CrossCuttingConcerns.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string key) : base($"Key not found: {key}")
        {
            Key = key;
        }

        public string Key { get; }
    }
}
