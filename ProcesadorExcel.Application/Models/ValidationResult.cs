namespace ProcesadorExcel.Application.Models
{
    public record ValidationResult(bool IsValid, string ErrorMessage = "");
}
