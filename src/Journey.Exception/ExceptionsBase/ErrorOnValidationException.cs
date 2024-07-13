using System.Net;

namespace Journey.Exception.ExceptionsBase;

public class ErrorOnValidationException : JourneyException
{
    private readonly List<string> _errors;

    public ErrorOnValidationException(List<string> messages) : base(string.Empty)
    {
        _errors = messages;
    }

    public override HttpStatusCode GetStatusCode()
    {
        return HttpStatusCode.BadRequest;
    }

    public override IList<string> GetErrorList()
    {
        return _errors;
    }
}