namespace Code4Nothing.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException() : base("One or more validation failures have occurred.")
    {
    }

    public ValidationException(string failures) : base(failures)
    {
    }
}
