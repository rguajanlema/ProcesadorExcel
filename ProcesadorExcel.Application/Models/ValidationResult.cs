using System;
using System.Collections.Generic;
using System.Text;

namespace ProcesadorExcel.Application.Models
{
    public record ValidationResult(bool IsValid, string ErrorMessage = "");
}
