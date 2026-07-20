namespace ProcesadorExcel.Application.Exceptions;

public sealed class ExcelValidationException : Exception
{
    public ExcelValidationException(string message)
        : base(message)
    {
    }
}
