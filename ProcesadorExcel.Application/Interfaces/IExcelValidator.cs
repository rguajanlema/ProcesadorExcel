using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProcesadorExcel.Application.Interfaces
{
    public interface IExcelValidator
    {
        // Recibe el path o stream del archivo
        ValidationResult ValidateStructure(string filePath, string[] expectedColumns);
    }
}
